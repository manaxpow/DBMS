using System;

public interface IDatabaseRegistry
{
    void RegisterDatabase(DatabaseDescriptor desc);
    void UnregisterDatabase(DatabaseId dbId);
    DatabaseDescriptor GetDatabase(DatabaseId dbId);
}
