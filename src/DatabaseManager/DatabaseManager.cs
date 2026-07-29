using DBMS.Exceptions;

public class DatabaseManager
{
    private CatalogManager _catalog;
    private Dictionary<string, Database> databases;

    public DatabaseManager()
    {
        this._catalog = new CatalogManager();
        this.databases = new Dictionary<string, Database>();
    }

    public static DatabaseManager Instance => throw new NotImplementedException();

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
