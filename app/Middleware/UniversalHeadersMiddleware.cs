namespace app.Middleware;

public class UniversalHeadersMiddleware
{
    private readonly RequestDelegate _next;

    public UniversalHeadersMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        context.Response.OnStarting(() =>
        {
            context.Response.Headers["X-Content-Type-Options"] = "nosniff";
            return Task.CompletedTask;
        });

        await _next(context);
    }
}