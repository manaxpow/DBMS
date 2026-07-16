using System;

public interface IStoredProcedureManager
{
    StoredProcedureId CreateProcedure(SchemaId schemaId, StoredProcedureDefinition def);
    void DropProcedure(StoredProcedureId procedureId);
}
