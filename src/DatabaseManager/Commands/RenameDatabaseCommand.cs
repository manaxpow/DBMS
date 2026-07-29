public class RenameDatabaseCommand : IDDLCommand
{
    private DatabaseManager _databaseManager;
    private string _databaseName;

    public RenameDatabaseCommand(DatabaseManager databaseManager, string databaseName)
    {
        this._databaseManager = databaseManager;
        this._databaseName = databaseName;
    }

    public DDLResult Execute()
    {
        throw new NotImplementedException();
    }
}
