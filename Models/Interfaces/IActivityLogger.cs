namespace Models;

public interface IActivityLogger
{
    IEnumerable<LogEvent> Get(long timestamp);
}