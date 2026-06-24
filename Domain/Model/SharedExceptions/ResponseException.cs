namespace Model.SharedExceptions;

public class ResponseException(string message, int statusCode) : Exception(message)
{
    public int StatusCode { get; } = statusCode;
}