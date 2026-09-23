using System.Security.Claims;

public class ApplicationLogService
    : IApplicationLogService
{
    private readonly IHttpContextAccessor
        _httpContextAccessor;

    public ApplicationLogService(
        IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor =
            httpContextAccessor;
    }

    public async Task LogAsync(
        string logLevel,
        string logMessage,
        string? exception,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var httpContext =
                _httpContextAccessor.HttpContext;

            int userID = 0;

            var userIdClaim =
                httpContext?
                    .User
                    .FindFirst(
                        ClaimTypes.NameIdentifier);

            if (int.TryParse(
                    userIdClaim?.Value,
                    out int currentUserID))
            {
                userID = currentUserID;
            }

            var ipAddress =
                httpContext?
                    .Connection
                    .RemoteIpAddress?
                    .ToString();

            var dto =
                new ApplicationLogsDTO
                {
                    UserID = userID,

                    LogLevel = logLevel,

                    LogMessage = logMessage,

                    Exception = exception,

                    IPAddress = ipAddress
                };

            await ApplicationLogsData
                .AddApplicationLogsAsync(
                    dto,
                    cancellationToken);
        }
        catch
        {
            // لا نكسر التطبيق إذا فشل تسجيل Log
        }
    }
}