public class DropDatabaseCommand : IDDLCommand
{
    private DatabaseManager databaseManager;
    private string databaseName;

    public DropDatabaseCommand(DatabaseManager databaseManager, string databaseName)
    {
        this.databaseManager = databaseManager;
        this.databaseName = databaseName;
    }

    public DDLResult Execute()
    {
        throw new NotImplementedException();
    }
}
