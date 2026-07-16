using System;

public class SystemCatalog : ISystemCatalog
{
    private object _cache;

    public TableDefinition GetTableDefinition(TableId tableId)
    {
        return default;
    }

    public IndexDefinition GetIndexDefinition(IndexId indexId)
    {
        return default;
    }

    public void InvalidateCache(CatalogObjectId id)
    {
    }

    public void FlushToDisk()
    {
    }
}
