public class CreateDatabaseCommand : IDDLCommand
{
    private DatabaseManager _databaseManager;
    private string _databaseName;

    public CreateDatabaseCommand(DatabaseManager databaseManager, string databaseName)
    {
        this._databaseManager = databaseManager;
        this._databaseName = databaseName;
    }

    public DDLResult Execute()
    {
        throw new NotImplementedException();
    }
}
