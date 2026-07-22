public class OnlineState : IDatabaseState
{
    private Database _database;

    public OnlineState(Database database)
    {
        _database = database;
    }
    public void Open() { }
    public void SetReadOnly() {
        _database.ChangeState(new ReadOnlyState(_database));
    }
    public void Recovery() { }
    public void Drop() {
        _database.ChangeState(new DroppedState(_database));
    }
}
