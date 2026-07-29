public class DropTableCommand : IDDLCommand
{
    private readonly Schema schema;
    private readonly string tableName;

    public DropTableCommand(Schema schema, string tableName)
    {
        this.schema = schema;
        this.tableName = tableName;
    }

    public DDLResult Execute()
    {
        throw new NotImplementedException();
    }
}
