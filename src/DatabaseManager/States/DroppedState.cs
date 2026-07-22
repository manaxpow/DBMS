public class DroppedState : IDatabaseState
{
    private Database _database;

    public DroppedState(Database database)
    {
        _database = database;
    }
    public void Open() { }
    public void SetReadOnly() { }
    public void Recovery() { }
    public void Drop() { }
}
