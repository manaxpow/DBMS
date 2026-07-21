public class SchemaObjectIterator : ISChemaObjectIterator
{
    private IReadOnlyList<ISchemaObject> _objects;
    private int _index;

    public SchemaObjectIterator(IReadOnlyList<ISchemaObject> objects)
    {
        _objects = objects;
        _index = 0;
    }
    public bool HasNext()
    {
        throw new NotImplementedException();
    }

    public ISchemaObject? Next() => throw new NotImplementedException();
}
