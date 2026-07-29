using System;

public class ReadOnlyState : IDatabaseState
{
    private Database database;

    public ReadOnlyState(Database database)
    {
        this.database = database;
    }

    public void Open()
    {
        throw new InvalidOperationException();
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
