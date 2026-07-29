public interface ISchemaObjectIterator
{
    public bool HasNext();

    public ISchemaObject? Next();

    public void Reset();
}
