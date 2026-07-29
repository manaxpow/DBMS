public class ViewIterator : ISchemaObjectIterator
{
    private IReadOnlyList<ISchemaObject> _views;

    public ViewIterator(IReadOnlyList<ISchemaObject> views)
    {
        this._views = views;
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
