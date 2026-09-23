public class ApplicationExceptionLoggingMiddleware
{
    private readonly RequestDelegate _next;

    public ApplicationExceptionLoggingMiddleware(
        RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(
        HttpContext context,
        IApplicationLogService applicationLogService)
    {
        try
        {
            await _next(context);
        }
        catch (OperationCanceledException)
            when (context.RequestAborted.IsCancellationRequested)
        {
            throw;
        }
        catch (Exception ex)
        {
            try
            {
                await applicationLogService.LogAsync(
                    "Error",
                    $"Unhandled exception occurred while processing {context.Request.Method} {context.Request.Path}.",
                    ex.ToString(),
                    CancellationToken.None);
            }
            catch
            {
                // لا نسمح لفشل الـ logging أن يخفي الـ exception الأصلي
            }

            throw;
        }
    }
}