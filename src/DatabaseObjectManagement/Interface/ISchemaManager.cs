using System;

public interface ISchemaManager
{
    SchemaId CreateSchema(string name, UserId ownerId);
    void DropSchema(SchemaId schemaId);
}
