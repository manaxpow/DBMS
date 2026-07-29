public class OnlineState : IDatabaseState
{
    private Database _database;

    public OnlineState(Database database)
    {
        this._database = database;
    }

    public void Open()
    {
    }

    public void SetReadOnly()
    {
        throw new NotImplementedException();
    }

    public void Recovery()
    {
    }

    public void Drop()
    {
        throw new NotImplementedException();
    }
}
