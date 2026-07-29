public class CreateDatabaseCommand : IDDLCommand
{
    private DatabaseManager databaseManager;
    private string databaseName;

    public CreateDatabaseCommand(DatabaseManager databaseManager, string databaseName)
    {
        this.databaseManager = databaseManager;
        this.databaseName = databaseName;
    }

    public DDLResult Execute()
    {
        throw new NotImplementedException();
    }
}
