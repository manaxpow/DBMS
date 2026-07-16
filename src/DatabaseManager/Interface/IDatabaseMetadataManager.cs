using System;

public interface IDatabaseMetadataManager
{
    DatabaseMetadata GetMetadata(DatabaseId dbId);
    void UpdateMetadata(DatabaseId dbId, DatabaseMetadata meta);
}
