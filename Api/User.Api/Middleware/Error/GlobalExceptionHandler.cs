using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace User.Api.Middleware.Error;

public sealed class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        logger?.LogError(exception, $"Error: {exception.Message}, time: {DateTime.UtcNow}");
        var problemDetails = new ProblemDetails()
        {
            Title = exception.GetType().Name,
            Detail = exception.Message,
            Status = StatusCodes.Status500InternalServerError,
            Instance = httpContext.Request.Path,
        };
        httpContext.Response.StatusCode = problemDetails.Status.Value;
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
        return true;
    }
}