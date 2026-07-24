public class OfflineState : IDatabaseState
{
    private Database _database;

    public OfflineState(Database database)
    {
        _database = database;
    }
    public void Open() {
        throw new NotImplementedException();
    }
    public void SetReadOnly() { }
    public void Recovery() { }
    public void Drop() { }
}

