using System;
using System.Linq;
using DBMS.Exceptions;

public class SchemaManager
{
    private CatalogManager catalogManager;
    private StorageEngine storageEngine;

    public SchemaManager()
    {
        this.catalogManager = new CatalogManager();
        this.storageEngine = new StorageEngine();
    }

    public SchemaManager(CatalogManager catalogManager, StorageEngine storageEngine)
    {
        this.catalogManager = catalogManager;
        this.storageEngine = storageEngine;
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
