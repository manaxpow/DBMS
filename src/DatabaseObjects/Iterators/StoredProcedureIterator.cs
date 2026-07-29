public class StoredProcedureIterator : ISchemaObjectIterator
{
    private IReadOnlyList<ISchemaObject> storedProcedures;

    public StoredProcedureIterator(IReadOnlyList<ISchemaObject> storedProcedures)
    {
        this.storedProcedures = storedProcedures;
    }

    public bool HasNext()
    {
        throw new NotImplementedException();
    }

    public ISchemaObject? Next()
    {
        throw new NotImplementedException();
    }

    public void Reset()
    {
        throw new NotImplementedException();
    }
}
