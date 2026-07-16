using System;

public interface IIndexDefinitionManager
{
    IndexId CreateIndex(TableId tableId, IndexDefinition def);
    void DropIndex(IndexId indexId);
}
