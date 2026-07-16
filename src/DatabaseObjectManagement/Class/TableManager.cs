using System;

public class TableManager : ITableManager
{
    private ISystemCatalog _catalog;

    public TableManager(ISystemCatalog catalog)
    {
        _catalog = catalog;
    }

    public TableId CreateTable(SchemaId schemaId, TableDefinition def)
    {
        return default;
    }

    public void DropTable(TableId tableId)
    {
    }

    public void AlterTable(TableId tableId, TableDefinition newDef)
    {
    }

    public void CheckConstraints()
    {
    }
}
