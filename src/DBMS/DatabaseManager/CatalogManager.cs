public class CatalogManager
{
    public void Register(ICatalogObject obj)
    {
        throw new NotImplementedException();
    }

    public T Find<T>(string name)
        where T : class, ICatalogObject
    {
        throw new NotImplementedException();
    }

    public void Remove(ICatalogObject obj)
    {
        throw new NotImplementedException();
    }
}
