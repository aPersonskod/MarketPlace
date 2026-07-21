namespace Model;

public class CachedObject<T>
{
    public CachedObject(T value, bool isSynced)
    {
        Value = value;
        IsSynced = isSynced;
    }

    public T Value { get; set; }
    public bool IsSynced { get; set; }
}