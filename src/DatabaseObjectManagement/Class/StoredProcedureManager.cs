using System;

public class StoredProcedureManager : IStoredProcedureManager
{
    public StoredProcedureId CreateProcedure(SchemaId schemaId, StoredProcedureDefinition def)
    {
        return default;
    }

    public void DropProcedure(StoredProcedureId procedureId)
    {
    }
}
