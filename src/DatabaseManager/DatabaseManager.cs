using DBMS.Exceptions;

public class DatabaseManager
{
    private CatalogManager _catalog;
    private static DatabaseManager _instance;
    public static DatabaseManager Instance => _instance ??= new DatabaseManager();
    private Dictionary<string, Database> _databases;

    public DatabaseManager()
    {
        _catalog = new CatalogManager();
        _databases = new Dictionary<string, Database>();
    }

    public void CreateDatabase(string name)
    {
        throw new DatabaseAlreadyExistsException();
    }

    public Database GetDatabase(string name)
    {
        throw new DatabaseNotFoundException();
    }

    public void DropDatabase(string name)
    {
        throw new DatabaseNotFoundException();
    }
}
