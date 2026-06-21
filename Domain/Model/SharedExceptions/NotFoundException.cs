namespace Model.SharedExceptions;

public class NotFoundException(string message) : Exception(message);