public class SchemaObjectsIterator : ISchemaObjectIterator
{
    private IReadOnlyList<ISchemaObject> objects;

    public SchemaObjectsIterator(IReadOnlyList<ISchemaObject> objects)
    {
        this.objects = objects;
    }

    public bool HasNext()
    {
        throw new NotImplementedException();
    }

    public ISchemaObject? Next() => throw new NotImplementedException();

    public void Reset() => throw new NotImplementedException();
}
