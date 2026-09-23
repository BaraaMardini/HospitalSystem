using System.Security.Claims;

public class SecurityLogService : ISecurityLogService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public SecurityLogService(
        IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task LogAsync(
      int? userID,
      string action,
      string description,
      CancellationToken cancellationToken = default)
    {
        try
        {
            var httpContext =
                _httpContextAccessor.HttpContext;

            // ==========================================
            // إذا لم يتم إرسال UserID
            // نحاول أخذه من JWT
            // ==========================================

            if (!userID.HasValue)
            {
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
            }

            // ==========================================
            // IP
            // ==========================================

            var ipAddress =
                httpContext?
                    .Connection
                    .RemoteIpAddress?
                    .ToString();

            // ==========================================
            // DTO
            // ==========================================

            var dto =
                new SecurityLogsDTO
                {
                    UserID = userID?? 0,
                    
                    Action = action,

                    Description = description,

                    IPAddress = ipAddress
                };

            await SecurityLogsData
                .AddSecurityLogsAsync(
                    dto,
                    cancellationToken);
        }
        catch
        {
            // Logging failure must not break application
        }
    }
}