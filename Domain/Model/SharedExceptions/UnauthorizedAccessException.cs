namespace Model.SharedExceptions;

public class UnauthorizedAccessException(string message) : Exception(message);