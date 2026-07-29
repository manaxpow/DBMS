public class AlterTableCommand : IDDLCommand
{
    private readonly Schema schema;
    private readonly string tableName;
    private ITableBuilder tableBuilder;

    public AlterTableCommand(Schema schema, string tableName, ITableBuilder tableBuilder)
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
