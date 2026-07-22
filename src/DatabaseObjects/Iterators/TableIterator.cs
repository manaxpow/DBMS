public class TableIterator : ISchemaObjectIterator
{
    private IReadOnlyList<ISchemaObject> _tables;
    public TableIterator(IReadOnlyList<ISchemaObject> tables)
    {
        _tables = tables;
    }

    public bool HasNext()
    {
        throw new NotImplementedException();
    }

    public ISchemaObject? Next()
    {
        throw new NotImplementedException();
    }
}
