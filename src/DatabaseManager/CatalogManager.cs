using DBMS.Exceptions;

public class CatalogManager
{
    private Dictionary<string, ICatalogObject> _store;

    public void Register(ICatalogObject obj)
    {
        throw new NotImplementedException();
    }

    public T Find<T>(string name) where T : class, ICatalogObject
    {
        throw new NotImplementedException();
    }

    public void Remove(ICatalogObject obj)
    {
        throw new NotImplementedException();
    }
}