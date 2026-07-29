public class ViewIterator : ISchemaObjectIterator
{
    private IReadOnlyList<ISchemaObject> views;

    public ViewIterator(IReadOnlyList<ISchemaObject> views)
    {
        this.views = views;
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
