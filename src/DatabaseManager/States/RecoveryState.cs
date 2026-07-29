public class RecoveryState : IDatabaseState
{
    private Database database;

    public RecoveryState(Database database)
    {
        this.database = database;
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
