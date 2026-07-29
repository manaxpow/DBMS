public class SchemaObjectsIterator : ISchemaObjectIterator
{
    private IReadOnlyList<ISchemaObject> _objects;

    public SchemaObjectsIterator(IReadOnlyList<ISchemaObject> objects)
    {
        this._objects = objects;
    }

    public bool HasNext()
    {
        throw new NotImplementedException();
    }

    public ISchemaObject? Next() => throw new NotImplementedException();

    public void Reset() => throw new NotImplementedException();
}
