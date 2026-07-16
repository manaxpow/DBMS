using System;

public class IndexDefinitionManager : IIndexDefinitionManager
{
    public IndexId CreateIndex(TableId tableId, IndexDefinition def)
    {
        return default;
    }

    public void DropIndex(IndexId indexId)
    {
    }

    public void ValidateIndexColumns()
    {
    }
}
