namespace Models;

public class Cart
{
    public IEnumerable<Order> Orders { get; set; }
}

public enum CartEventType
{
    OrderAdded,
    OrderChanged,
    OrderRemoved
}

public class CartEvent : EventBase
{
    public CartEventType Type { get; set; }
    public Order Order { get; set; }
}

public abstract class EventBase
{
    private static long TimeStampBase = new DateTime(2000, 1, 1).Ticks;
    public long TimeStamp { get; set; }
    public DateTime Time { get; set; }
    public EventBase()
    {
        Time = DateTime.Now;
        TimeStamp = Time.Ticks - TimeStampBase;
    }
}