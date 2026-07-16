using System;

public interface ITableManager
{
    TableId CreateTable(SchemaId schemaId, TableDefinition def);
    void DropTable(TableId tableId);
    void AlterTable(TableId tableId, TableDefinition newDef);
}
