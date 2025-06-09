using Backend.Data;
using Microsoft.AspNetCore.Http;

namespace Backend.Middleware;

// Handle cross-cutting concerns like logging or authentication
// Logs request and response details. Call app.UseCustomMiddleware() in Program.cs.
public class CustomMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<CustomMiddleware> _logger;

    public CustomMiddleware(RequestDelegate next, ILogger<CustomMiddleware> logger)
    {
        _next = next ?? throw new ArgumentNullException(nameof(next));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            _logger.LogInformation("Request received: {Method} {Path}", context.Request.Method, context.Request.Path);
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Middleware processing failed for request {Method} {Path}", context.Request.Method, context.Request.Path);
            throw;
        }
        finally
        {
            _logger.LogInformation("Response sent: {StatusCode}", context.Response.StatusCode);
        }
    }
}

public static class CustomMiddlewareExtensions
{
    public static IApplicationBuilder UseCustomMiddleware(this IApplicationBuilder builder)
    {
        try
        {
            return builder.UseMiddleware<CustomMiddleware>();
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "Failed to register CustomMiddleware");
            throw;
        }
    }
}