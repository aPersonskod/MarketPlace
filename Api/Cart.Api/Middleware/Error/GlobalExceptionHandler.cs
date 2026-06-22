using Grpc.Core;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Model.SharedExceptions;

namespace Cart.Api.Middleware.Error;

public sealed class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        logger?.LogError(exception, $"Error: {exception.Message}, time: {DateTime.UtcNow}");
        
        var exceptionDetails = exception switch
        {
            NotFoundException => new ExceptionDetails(StatusCodes.Status404NotFound, "Not Found", exception.Message),
            ArgumentException => new ExceptionDetails(StatusCodes.Status400BadRequest, "Bad Request", exception.Message),
            RpcException rpc => new ExceptionDetails(StatusCodes.Status400BadRequest, "Bad Request", rpc.Status.Detail),
            _ => new ExceptionDetails(StatusCodes.Status500InternalServerError, "Internal Server Error", exception.Message)
        };

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

public record ExceptionDetails(int StatusCode, string Title, string Detail);