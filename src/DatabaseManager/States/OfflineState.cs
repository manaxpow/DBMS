public class OfflineState : IDatabaseState
{
    private Database _database;

    public OfflineState(Database database)
    {
        _database = database;
    }
    public void Open() {
        _database.ChangeState(new OnlineState(_database));
    }
    public void SetReadOnly() { }
    public void Recovery() { }
    public void Drop() { }
}
