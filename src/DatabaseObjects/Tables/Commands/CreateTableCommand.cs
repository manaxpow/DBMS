public class CreateTableCommand : IDDLCommand
{
    private readonly Schema schema;
    private readonly string tableName;
    private ITableBuilder tableBuilder;

    public CreateTableCommand(Schema schema, string tableName, ITableBuilder tableBuilder)
    {
        this.schema = schema;
        this.tableName = tableName;
        this.tableBuilder = tableBuilder;
    }

    public DDLResult Execute()
    {
        throw new NotImplementedException();
    }
}
