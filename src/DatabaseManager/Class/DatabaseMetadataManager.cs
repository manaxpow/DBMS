using System;

public class DatabaseMetadataManager : IDatabaseMetadataManager
{
    private object _repo;

    public DatabaseMetadata GetMetadata(DatabaseId dbId)
    {
        return default;
    }

    public void UpdateMetadata(DatabaseId dbId, DatabaseMetadata meta)
    {
    }

    public void SyncVersion()
    {
    }
}
