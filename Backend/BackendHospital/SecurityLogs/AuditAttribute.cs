using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Infrastructure;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public class AuditAttribute : Attribute, IAsyncActionFilter
{
    private readonly string _action;

    public AuditAttribute(string action)
    {
        _action = action;
    }

    public async Task OnActionExecutionAsync(
        ActionExecutingContext context,
        ActionExecutionDelegate next)
    {
        // ==========================================
        // محاولة قراءة الـ ID المستهدف من الـ route
        // (شغال تلقائياً مع أي Action فيه {id})
        // ==========================================
        object? targetId = null;

        if (context.ActionArguments.TryGetValue("id", out var idValue))
        {
            targetId = idValue;
        }

        var executedContext = await next();

        int statusCode = 200;
        if (executedContext.Result is IStatusCodeActionResult statusResult)
        {
            statusCode = statusResult.StatusCode ?? 200;
        }

        bool success = statusCode >= 200 && statusCode < 300;

        var securityLogService =
            context.HttpContext.RequestServices
                .GetRequiredService<ISecurityLogService>();

        var targetInfo =
            targetId != null ? $" (Target ID: {targetId})" : "";

        var description = success
            ? $"{_action} executed successfully.{targetInfo}"
            : $"{_action} failed. StatusCode: {statusCode}.{targetInfo}";

        await securityLogService.LogAsync(
            null,
            _action,
            description,
            CancellationToken.None);
    }
}