public class StoredProcedureIterator : ISchemaObjectIterator
{
    private IReadOnlyList<ISchemaObject> _storedProcedures;
    public StoredProcedureIterator(IReadOnlyList<ISchemaObject> storedProcedures)
    {
        _storedProcedures = storedProcedures;
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
