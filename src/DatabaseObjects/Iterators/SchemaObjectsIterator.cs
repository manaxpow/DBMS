public class SchemaObjectsIterator : ISchemaObjectIterator
{
    private IReadOnlyList<ISchemaObject> _objects;
    private int _index;

    public SchemaObjectsIterator(IReadOnlyList<ISchemaObject> objects)
    {
        _objects = objects;
        _index = 0;
    }
    public bool HasNext()
    {
        throw new NotImplementedException();
    }

    public ISchemaObject? Next() => throw new NotImplementedException();

    public void Reset() => throw new NotImplementedException();
}
