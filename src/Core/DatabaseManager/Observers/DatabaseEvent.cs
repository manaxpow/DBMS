public class DatabaseEvent
{
    public DatabaseEventType Type { get; set; }

    public string DatabaseName { get; set; } = null!;

    public DateTime Timestamp { get; set; }
}
