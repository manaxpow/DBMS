public class RenameDatabaseCommand : IDDLCommand
{
    private DatabaseManager databaseManager;
    private string databaseName;

    public RenameDatabaseCommand(DatabaseManager databaseManager, string databaseName)
    {
        this.databaseManager = databaseManager;
        this.databaseName = databaseName;
    }

    public DDLResult Execute()
    {
        throw new NotImplementedException();
    }
}
