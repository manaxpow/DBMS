public class DroppedState : IDatabaseState
{
    private Database database;

    public DroppedState(Database database)
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
