using Microsoft.AspNetCore.Mvc;
using BackendHospital.Models;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Security.Cryptography;
using Microsoft.AspNetCore.RateLimiting;
using System.Runtime.InteropServices;
using StudentApi.DTOs.Auth;

[Route("api/LoginRequest")]
[ApiController]
[EnableRateLimiting("AuthLimiter")]
public class LoginRequestController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly ISecurityLogService _securityLogService;



    public LoginRequestController(
        IConfiguration configuration,
        ISecurityLogService securityLogService)
    {
        _configuration = configuration;
        _securityLogService = securityLogService;
    }
    

    // =====================================================
    // Generate Random Refresh Secret
    // =====================================================

    private static string GenerateRefreshSecret()
    {
        var bytes = new byte[64];

        RandomNumberGenerator.Fill(bytes);

        return Convert.ToBase64String(bytes);
    }


    // =====================================================
    // Build Refresh Token
    //
    // Format:
    // ID.SECRET
    // =====================================================

    private static string BuildRefreshToken(
        int id,
        string secret)
    {
        return $"{id}.{secret}";
    }


    // =====================================================
    // LOGIN
    // =====================================================

    [ProducesResponseType(
        typeof(TokenResponse),
        StatusCodes.Status200OK)]

    [ProducesResponseType(
        StatusCodes.Status401Unauthorized)]

    [ProducesResponseType(
        StatusCodes.Status500InternalServerError)]

    [HttpPost("Login")]
    [Audit("Login")]

    public async Task<ActionResult<TokenResponse>> LoginRequest(
        [FromBody] LoginRequest dto,
        CancellationToken cancellationToken)
    {
        // =================================================
        // Validate User + Password
        // =================================================

        var result =
            await LoginRequestService.LoginRequest(
                dto,
                cancellationToken);
 

        // =================================================
        // Database Error
        // =================================================

        if (result.ErrorType ==
            ErrorType.DatabaseError)
        {
            return StatusCode(
                StatusCodes.Status500InternalServerError,
                result);
        }

        // =================================================
        // Invalid Credentials

        // =================================================
       
        if (result.ErrorType != ErrorType.None)
        {
          

            return Unauthorized("Invalid username or password.");
        }


        // =================================================
        // Generate Refresh Secret
        // =================================================

        var refreshSecret =
            GenerateRefreshSecret();


        // =================================================
        // Hash Refresh Secret
        // =================================================

        var refreshTokenHash =
            BCrypt.Net.BCrypt.HashPassword(
                refreshSecret);


        // =================================================
        // Refresh Token Expiration
        // =================================================

        var refreshExpiresAt =
            DateTime.UtcNow.AddDays(7);


        // =================================================
        // Save Refresh Token
        // =================================================

        var refreshResult =
            await RefreshTokenService.Create(
                result.Data.UserID,
                refreshTokenHash,
                refreshExpiresAt,
                cancellationToken);


        // =================================================
        // Database Error
        // =================================================

        if (refreshResult.ErrorType ==
            ErrorType.DatabaseError)
        {
            return StatusCode(
                StatusCodes.Status500InternalServerError,
                refreshResult);
        }


        // =================================================
        // Failed To Create Refresh Token
        // =================================================

        if (refreshResult.Data <= 0)
        {
            return StatusCode(
                StatusCodes.Status500InternalServerError,
                "Failed to create refresh token.");
        }


        // =================================================
        // Build Refresh Token
        // =================================================

        var refreshToken =
            BuildRefreshToken(
                refreshResult.Data,
                refreshSecret);


        // =================================================
        // Create Access Token
        // =================================================

        var accessToken =
            CreateAccessToken(
                result.Data);


        // =================================================
        // Return Tokens
        // =================================================

        return Ok(
            new TokenResponse
            {
                AccessToken =
                    accessToken,

                RefreshToken =
                    refreshToken
            });
    }


    // =====================================================
    // REFRESH
    // =====================================================

    [ProducesResponseType(
        typeof(TokenResponse),
        StatusCodes.Status200OK)]

    [ProducesResponseType(
        StatusCodes.Status401Unauthorized)]

    [ProducesResponseType(
        StatusCodes.Status500InternalServerError)]

    [HttpPost("refresh")]
    [Audit("RefreshToken")]

    public async Task<ActionResult<TokenResponse>> Refresh(
        [FromBody] RefreshRequest dto,
        CancellationToken cancellationToken)
    {
        // =================================================
        // Validate Request
        // =================================================

        if (dto == null ||
            string.IsNullOrWhiteSpace(
                dto.RefreshToken))
        {
            return Unauthorized(
                "Invalid refresh token.");
        }


        // =================================================
        // Split Token
        //
        // Format:
        //
        // ID.SECRET
        // =================================================

        var parts =
            dto.RefreshToken.Split(
                '.',
                2);


        if (parts.Length != 2)
        {
            return Unauthorized(
                "Invalid refresh token.");
        }


        // =================================================
        // Parse Refresh Token ID
        // =================================================

        if (!int.TryParse(
                parts[0],
                out int refreshTokenId))
        {
            return Unauthorized(
                "Invalid refresh token.");
        }


        if (refreshTokenId <= 0)
        {
            return Unauthorized(
                "Invalid refresh token.");
        }


        // =================================================
        // Get Secret
        // =================================================

        var refreshSecret =
            parts[1];


        if (string.IsNullOrWhiteSpace(
                refreshSecret))
        {
            return Unauthorized(
                "Invalid refresh token.");
        }


        // =================================================
        // Get Refresh Token
        // =================================================

        var refreshResult =
            await RefreshTokenService.GetByID(
                refreshTokenId,
                cancellationToken);


        // =================================================
        // Database Error
        // =================================================

        if (refreshResult.ErrorType ==
            ErrorType.DatabaseError)
        {
            return StatusCode(
                StatusCodes.Status500InternalServerError,
                refreshResult);
        }


        // =================================================
        // Token Not Found
        // =================================================

        if (refreshResult.Data == null)
        {
            return Unauthorized(
                "Invalid refresh token.");
        }


        // =================================================
        // Check Revoked
        // =================================================

        if (refreshResult.Data.RevokedAt != null)
        {
            return Unauthorized(
                "Invalid refresh token.");
        }


        // =================================================
        // Check Expiration
        // =================================================

        if (refreshResult.Data.ExpiresAt <=
            DateTime.UtcNow)
        {
            return Unauthorized(
                "Invalid refresh token.");
        }


        // =================================================
        // Verify Secret
        // =================================================

        bool refreshValid;

        try
        {
            refreshValid =
                BCrypt.Net.BCrypt.Verify(
                    refreshSecret,
                    refreshResult.Data.RefreshTokenHash);
        }
        catch
        {
            return Unauthorized(
                "Invalid refresh token.");
        }


        if (!refreshValid)
        {
            return Unauthorized(
                "Invalid refresh token.");
        }


        // =================================================
        // Get User
        // =================================================

        var userResult =
            await LoginRequestService.GetUserByID(
                refreshResult.Data.UserID,
                cancellationToken);


        // =================================================
        // Database Error
        // =================================================

        if (userResult.ErrorType ==
            ErrorType.DatabaseError)
        {
            return StatusCode(
                StatusCodes.Status500InternalServerError,
                userResult);
        }


        // =================================================
        // User Not Found
        // =================================================

        if (userResult.Data == null)
        {
            return Unauthorized(
                "Invalid refresh token.");
        }


        // =================================================
        // Generate New Refresh Secret
        // =================================================

        var newRefreshSecret =
            GenerateRefreshSecret();


        // =================================================
        // Hash New Refresh Secret
        // =================================================

        var newRefreshTokenHash =
            BCrypt.Net.BCrypt.HashPassword(
                newRefreshSecret);


        // =================================================
        // New Expiration
        // =================================================

        var newRefreshExpiresAt =
            DateTime.UtcNow.AddDays(7);


        // =================================================
        // Rotate Refresh Token
        // =================================================

        var rotationResult =
            await RefreshTokenService.Rotate(
                refreshTokenId,
                refreshResult.Data.UserID,
                newRefreshTokenHash,
                newRefreshExpiresAt,
                cancellationToken);


        // =================================================
        // Database Error
        // =================================================

        if (rotationResult.ErrorType ==
            ErrorType.DatabaseError)
        {
            return StatusCode(
                StatusCodes.Status500InternalServerError,
                rotationResult);
        }


        // =================================================
        // Rotation Failed
        // =================================================

        if (rotationResult.Data <= 0)
        {
            return StatusCode(
                StatusCodes.Status500InternalServerError,
                "Failed to rotate refresh token.");
        }


        // =================================================
        // Build New Refresh Token
        // =================================================

        var newRefreshToken =
            BuildRefreshToken(
                rotationResult.Data,
                newRefreshSecret);


        // =================================================
        // Create New Access Token
        // =================================================

        var newAccessToken =
            CreateAccessToken(
                userResult.Data);


        // =================================================
        // Return Tokens
        // =================================================

        return Ok(
            new TokenResponse
            {
                AccessToken =
                    newAccessToken,

                RefreshToken =
                    newRefreshToken
            });
    }


    // =====================================================
    // Create Access Token
    // =====================================================

    private string CreateAccessToken(
        UserInfo user)
    {
        var claims =
            new[]
            {
                new Claim(
                    ClaimTypes.NameIdentifier,
                    user.UserID.ToString()),

                new Claim(
                    ClaimTypes.Name,
                    user.UserName ?? string.Empty),

                new Claim(
                    ClaimTypes.Role,
                    user.Role ?? string.Empty),

                new Claim(
                    "PermissionMask1",
                    user.PermissionMask1.ToString()),

                new Claim(
                    "PermissionMask2",
                    user.PermissionMask2.ToString())
            };


        // =================================================
        // Read JWT Configuration
        // =================================================

        var jwtKey =
            _configuration["Jwt:Key"];

        var jwtIssuer =
            _configuration["Jwt:Issuer"];

        var jwtAudience =
            _configuration["Jwt:Audience"];


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


        // =================================================
        // Security Key
        // =================================================

        var key =
            new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(
                    jwtKey));


        var credentials =
            new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256);


        // =================================================
        // Create JWT
        // =================================================

        var token =
            new JwtSecurityToken(
                issuer:
                    jwtIssuer,

                audience:
                    jwtAudience,

                claims:
                    claims,

                notBefore:
                    DateTime.UtcNow,

                expires:
                    DateTime.UtcNow.AddMinutes(120),

                signingCredentials:
                    credentials);


        return new JwtSecurityTokenHandler()
            .WriteToken(token);
    }

    [HttpPost("logout")]
    [Audit("Logout")]
    public async Task<IActionResult> Logout(
    [FromBody] LogoutRequest request,
    CancellationToken cancellationToken)
    {
        if (request == null ||
            string.IsNullOrWhiteSpace(request.RefreshToken))
        {
            return Unauthorized("Invalid refresh token.");
        }

        // ID.SECRET
        var parts =
            request.RefreshToken.Split('.', 2);

        if (parts.Length != 2 ||
            !int.TryParse(
                parts[0],
                out int refreshTokenId))
        {
            return Unauthorized("Invalid refresh token.");
        }

        // Get Refresh Token
        var refreshResult =
            await RefreshTokenService.GetByID(
                refreshTokenId,
                cancellationToken);

        if (refreshResult.ErrorType ==
            ErrorType.DatabaseError)
        {
            return StatusCode(
                StatusCodes.Status500InternalServerError,
                refreshResult);
        }

        if (refreshResult.Data == null)
        {
            return Unauthorized("Invalid refresh token.");
        }

        // Get User
        var userResult =
            await LoginRequestService.GetUserByID(
                refreshResult.Data.UserID,
                cancellationToken);

        if (userResult.ErrorType ==
            ErrorType.DatabaseError)
        {
            return StatusCode(
                StatusCodes.Status500InternalServerError,
                userResult);
        }

        if (userResult.Data == null)
        {
            return Unauthorized("Invalid user.");
        }

        // Revoke Token
        var revokeResult =
            await RefreshTokenService.Revoke(
                refreshTokenId,
                userResult.Data.UserID,
                cancellationToken);

        if (revokeResult.ErrorType ==
            ErrorType.DatabaseError)
        {
            return StatusCode(
                StatusCodes.Status500InternalServerError,
                revokeResult);
        }

        if (revokeResult.Data <= 0)
        {
            return Unauthorized(
                "Invalid refresh token.");
        }

        return Ok(
            "Logged out successfully.");
    }
}