using System;

public class ReadOnlyState : IDatabaseState
{
    private Database _database;

    public ReadOnlyState(Database database)
    {
        _database = database;
    }
    public void Open() {
        throw new InvalidOperationException();
    }
    public void SetReadOnly() { }
    public void Recovery() { }
    public void Drop() { }
}
