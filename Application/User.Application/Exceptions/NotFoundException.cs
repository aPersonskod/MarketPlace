namespace User.Application.Exceptions;

public class NotFoundException(string message) : Exception(message);
public class UnauthorizedAccessException(string message) : Exception(message);