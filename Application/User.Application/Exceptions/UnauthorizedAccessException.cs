namespace User.Application.Exceptions;

public class UnauthorizedAccessException(string message) : Exception(message);