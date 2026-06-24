using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Model.SharedExceptions;
using Model.SharedExceptions.ProblemDetails;

namespace Product.Api.Middleware.Error;

public sealed class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        logger?.LogError(exception, $"Error: {exception.Message}, time: {DateTime.UtcNow}");

        var exceptionDetails = exception.GetExceptionDetails();

        var problemDetails = new ProblemDetails()
        {
            Status = exceptionDetails.StatusCode,
            Title = exceptionDetails.Title, //exception.GetType().Name,
            Detail = exceptionDetails.Detail,
            Instance = httpContext.Request.Path,
        };
        
        httpContext.Response.StatusCode = problemDetails.Status.Value;
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
        return true;
    }
}