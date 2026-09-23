using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);


// ===============================
// CORS Configuration
// ===============================

builder.Services.AddCors(options =>
{
    options.AddPolicy("ReactApp", policy =>
    {
        policy
            .WithOrigins("http://localhost:5173")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});


// ===============================
// JWT Configuration
// ===============================

var jwtKey =
    builder.Configuration["Jwt:Key"];

var jwtIssuer =
    builder.Configuration["Jwt:Issuer"];

var jwtAudience =
    builder.Configuration["Jwt:Audience"];


if (string.IsNullOrWhiteSpace(jwtKey))
{
    throw new InvalidOperationException(
        "JWT key is not configured.");
}

if (string.IsNullOrWhiteSpace(jwtIssuer))
{
    throw new InvalidOperationException(
        "JWT issuer is not configured.");
}

if (string.IsNullOrWhiteSpace(jwtAudience))
{
    throw new InvalidOperationException(
        "JWT audience is not configured.");
}


// ===============================
// JWT Authentication
// ===============================

builder.Services
    .AddAuthentication(
        JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters =
            new TokenValidationParameters
            {
                ValidateIssuer = true,

                ValidateAudience = true,

                ValidateLifetime = true,

                ValidateIssuerSigningKey = true,

                ValidIssuer = jwtIssuer,

                ValidAudience = jwtAudience,

                IssuerSigningKey =
                    new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwtKey)),

                ClockSkew = TimeSpan.Zero
            };
    });


// ===============================
// Authorization
// ===============================

builder.Services.AddAuthorization();

builder.Services.AddSingleton<
    IAuthorizationPolicyProvider,
    PermissionPolicyProvider>();

builder.Services.AddSingleton<
    IAuthorizationHandler,
    PermissionHandler>();


// ===============================
// Rate Limiting
// ===============================

builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode =
        StatusCodes.Status429TooManyRequests;

    options.AddPolicy("AuthLimiter", httpContext =>
    {
        var ip =
            httpContext.Connection.RemoteIpAddress?
                .ToString()
            ?? "unknown";

        return RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: ip,
            factory: _ =>
                new FixedWindowRateLimiterOptions
                {
                    PermitLimit = 5,

                    Window =
                        TimeSpan.FromMinutes(1),

                    QueueLimit = 0,

                    AutoReplenishment = true
                });
    });
});


// ===============================
// Controllers
// ===============================

builder.Services.AddControllers();

builder.Services.AddHttpContextAccessor();


// ===============================
// Logging Services
// ===============================

builder.Services.AddScoped<
    IApplicationLogService,
    ApplicationLogService>();

builder.Services.AddScoped<
    ISecurityLogService,
    SecurityLogService>();


// ===============================
// Swagger
// ===============================

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition(
        "Bearer",
        new OpenApiSecurityScheme
        {
            Name = "Authorization",

            Type =
                SecuritySchemeType.Http,

            Scheme = "bearer",

            BearerFormat = "JWT",

            In =
                ParameterLocation.Header,

            Description =
                "Enter your JWT token"
        });

    options.AddSecurityRequirement(
        new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference =
                        new OpenApiReference
                        {
                            Type =
                                ReferenceType.SecurityScheme,

                            Id = "Bearer"
                        }
                },

                Array.Empty<string>()
            }
        });
});


// ===============================
// Build
// ===============================

var app = builder.Build();


// ===============================
// Development
// ===============================

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    app.UseSwaggerUI();
}


// ===============================
// Middleware
// ===============================

app.UseHttpsRedirection();

app.UseCors("ReactApp");

app.UseMiddleware<
    ApplicationExceptionLoggingMiddleware>();

app.UseRateLimiter();

app.UseAuthentication();

app.UseAuthorization();


// ===============================
// Controllers
// ===============================

app.MapControllers();


app.Run();