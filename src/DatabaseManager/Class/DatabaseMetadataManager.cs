using System;

public class DatabaseMetadataManager : IDatabaseMetadataManager
{

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
