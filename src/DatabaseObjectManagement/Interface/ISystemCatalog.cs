using System;

public interface ISystemCatalog
{
    TableDefinition GetTableDefinition(TableId tableId);
    IndexDefinition GetIndexDefinition(IndexId indexId);
    void InvalidateCache(CatalogObjectId id);
}
