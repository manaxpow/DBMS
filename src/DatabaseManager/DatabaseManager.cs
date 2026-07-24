using DBMS.Exceptions;

public class DatabaseManager
{
    private CatalogManager _catalog;
    private static DatabaseManager _instance;
    public static DatabaseManager Instance => throw new NotImplementedException();
    private Dictionary<string, Database> _databases;

    public DatabaseManager()
    {
        _catalog = new CatalogManager();
        _databases = new Dictionary<string, Database>();
    }

    public void CreateDatabase(string name)
    {
        throw new NotImplementedException();
    }

    public Database GetDatabase(string name)
    {
        throw new NotImplementedException();
    }

    public void DropDatabase(string name)
    {
        throw new NotImplementedException();
    }
}
