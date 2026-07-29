public class TableIterator : ISchemaObjectIterator
{
    private IReadOnlyList<ISchemaObject> _tables;

    public TableIterator(IReadOnlyList<ISchemaObject> tables)
    {
        this._tables = tables;
    }

    public bool HasNext()
    {
        throw new NotImplementedException();
    }

    public ISchemaObject? Next()
    {
        throw new NotImplementedException();
    }

    public void Reset() => throw new NotImplementedException();
}
