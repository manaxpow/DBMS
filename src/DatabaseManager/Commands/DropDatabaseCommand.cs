public class DropDatabaseCommand : IDDLCommand
{
    private DatabaseManager _databaseManager;
    private string _databaseName;

    public DropDatabaseCommand(DatabaseManager databaseManager, string databaseName)
    {
        _databaseManager = databaseManager;
        _databaseName = databaseName;
    }
    public DDLResult Execute()
    {
        return DDLResult.Success;
    }
}
