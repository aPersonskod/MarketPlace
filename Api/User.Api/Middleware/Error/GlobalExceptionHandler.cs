using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Model.SharedExceptions;
using Model.SharedExceptions.ProblemDetails;

namespace User.Api.Middleware.Error;

public sealed class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        logger?.LogError(exception, $"Error: {exception.Message}, time: {DateTime.UtcNow}");
        
        var exceptionDetails = exception switch
        {
            NotFoundException => new ExceptionDetails(StatusCodes.Status404NotFound, "Not Found", exception.Message),
            FluentValidation.ValidationException => new ExceptionDetails(StatusCodes.Status400BadRequest, "Bad Request", exception.Message),
            _ => exception.GetExceptionDetails()
        };

        var problemDetails = new ProblemDetails()
        {
            Status = exceptionDetails.StatusCode,
            Title = exceptionDetails.Title, //exception.GetType().Name,
            Detail = exceptionDetails.Detail,
            Instance = httpContext.Request.Path,
        };
        
        if (exception is FluentValidation.ValidationException validationException)
        {
            var errors = validationException.Errors
                .GroupBy(e => e.PropertyName)
                .ToDictionary(
                    failureGroup => failureGroup.Key,
                    failureGroup => failureGroup.Select(f => f.ErrorMessage).ToArray()
                );
            problemDetails.Extensions.Add("errors", errors);
        }
        
        httpContext.Response.StatusCode = problemDetails.Status.Value;
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
        return true;
    }
}