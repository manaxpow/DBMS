using System;
using System.Linq;
using DBMS.Exceptions;

public class SchemaManager
{
    private CatalogManager _catalogManager;
    private StorageEngine _storageEngine;

    public SchemaManager()
    {
        this._catalogManager = new CatalogManager();
        this._storageEngine = new StorageEngine();
    }

    public SchemaManager(CatalogManager catalogManager, StorageEngine storageEngine)
    {
        this._catalogManager = catalogManager;
        this._storageEngine = storageEngine;
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
