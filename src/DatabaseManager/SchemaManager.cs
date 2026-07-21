using System;
using System.Linq;
using DBMS.Exceptions;

public class SchemaManager
{
    private CatalogManager _catalogManager;
    private StorageEngine _storageEngine;

    public SchemaManager()
    {
        _catalogManager = new CatalogManager();
        _storageEngine = new StorageEngine();
    }

    public SchemaManager(CatalogManager catalogManager, StorageEngine storageEngine)
    {
        _catalogManager = catalogManager;
        _storageEngine = storageEngine;
    }

    public void DropSchema(Schema schema, bool cascade = false)
    {
        throw new NotImplementedException();
    }

    private void DropObject(Schema schema, ISchemaObject schemaObject)
    {
        throw new NotImplementedException();
    }

    private void CheckTableDependencies()
    {
        // Stub
    }

    private void CheckViewDependencies()
    {
        // Stub
    }
}
