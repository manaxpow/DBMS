public class RecoveryState : IDatabaseState
{
    private Database _database;

    public RecoveryState(Database database)
    {
        this._database = database;
    }

    public void Open()
    {
    }

    public void SetReadOnly()
    {
    }

    public void Recovery()
    {
    }

    public void Drop()
    {
    }
}
