public class CreateDatabaseCommand : IDDLCommand
{
    private DatabaseManager _databaseManager;
    private string _databaseName;

    public CreateDatabaseCommand(DatabaseManager databaseManager, string databaseName)
    {
        _databaseManager = databaseManager;
        _databaseName = databaseName;
    }
    public DDLResult Execute()
    {
        throw new NotImplementedException();
    }
}

