using System;

public class DatabaseRegistry : IDatabaseRegistry
{

    public void RegisterDatabase(DatabaseDescriptor desc)
    {
    }

    public void UnregisterDatabase(DatabaseId dbId)
    {
    }

    public DatabaseDescriptor GetDatabase(DatabaseId dbId)
    {
        return default;
    }

    public void ResolveName()
    {
    }
}
