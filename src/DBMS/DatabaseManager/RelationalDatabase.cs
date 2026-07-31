using DBMS.Exceptions;

public class RelationalDatabase : Database
{
    public RelationalDatabase()
    {
    }

    public RelationalDatabase(IStorageEngine storageEngine)
        : base(storageEngine)
    {
    }

    public void ChangeState(IDatabaseState state)
    {
        var field = typeof(Database).GetField("_state", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        field!.SetValue(this, state);
    }

    public void Open()
    {
        throw new NotImplementedException();
    }

    public void SetReadOnly()
    {
        throw new NotImplementedException();
    }

    public void Recovery()
    {
        throw new NotImplementedException();
    }

    public void Drop()
    {
        throw new NotImplementedException();
    }

    public void Close()
    {
        throw new FlushFailureException();
    }

    // Base class handles AddSchema and DropSchema

    public override void Initialize()
    {
        throw new NotImplementedException();
    }
}
