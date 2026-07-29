public class DropDatabaseCommand : IDDLCommand
{
    private DatabaseManager _databaseManager;
    private string _databaseName;

    public DropDatabaseCommand(DatabaseManager databaseManager, string databaseName)
    {
        this._databaseManager = databaseManager;
        this._databaseName = databaseName;
    }

    public DDLResult Execute()
    {
        throw new NotImplementedException();
    }
}
