public interface IApplicationLogService
{
    Task LogAsync(
        string logLevel,
        string logMessage,
        string? exception,
        CancellationToken cancellationToken = default);
}