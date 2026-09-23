public interface ISecurityLogService
{
    Task LogAsync(
        int? userID,
        string action,
        string description,
        CancellationToken cancellationToken = default);
}