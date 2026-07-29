public class CreateTableCommand : IDDLCommand
{
    private readonly Schema _schema;
    private readonly string _tableName;
    private ITableBuilder _tableBuilder;

    public CreateTableCommand(Schema schema, string tableName, ITableBuilder tableBuilder)
    {
        this._schema = schema;
        this._tableName = tableName;
        this._tableBuilder = tableBuilder;
    }

    public DDLResult Execute()
    {
        throw new NotImplementedException();
    }
}
