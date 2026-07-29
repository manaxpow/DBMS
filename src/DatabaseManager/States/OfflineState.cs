public class OfflineState : IDatabaseState
{
    private Database database;

    public OfflineState(Database database)
    {
        this.database = database;
    }

    public void Open()
    {
        throw new NotImplementedException();
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
