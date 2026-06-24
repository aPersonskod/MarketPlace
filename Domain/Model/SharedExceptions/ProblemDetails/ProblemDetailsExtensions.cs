using System.Net;

namespace Model.SharedExceptions.ProblemDetails;

public static class ExceptionDetailsExtensions
{
    public static ExceptionDetails GetExceptionDetails(this Exception exception)
    {
        return exception switch
        {
            NotFoundException => new ExceptionDetails((int)HttpStatusCode.NotFound, "Not Found", exception.Message),
            ArgumentException => new ExceptionDetails((int)HttpStatusCode.BadRequest, "Bad Request", exception.Message),
            NoContentException => new ExceptionDetails((int)HttpStatusCode.NoContent, "No Content", exception.Message),
            ResponseException r => new ExceptionDetails(r.StatusCode, "Api response", r.Message),
            _ => new ExceptionDetails((int)HttpStatusCode.InternalServerError, "Internal Server Error", exception.Message)
        };
    }
}

public record ExceptionDetails(int StatusCode, string Title, string Detail);