using System;

public class SchemaManager : ISchemaManager
{
    private ISystemCatalog _catalog;

    public SchemaManager(ISystemCatalog catalog)
    {
        _catalog = catalog;
    }

    public SchemaId CreateSchema(string name, UserId ownerId)
    {
        return default;
    }

    public void DropSchema(SchemaId schemaId)
    {
    }

    public void ValidateSchema()
    {
    }
}
