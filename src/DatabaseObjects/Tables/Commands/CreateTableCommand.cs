public class CreateTableCommand : IDDLCommand
{
    private readonly Schema _schema;
    private readonly string _tableName;

    public CreateTableCommand(Schema schema, string tableName)
    {
        _schema = schema;
        _tableName = tableName;
    }

    public DDLResult Execute()
    {
        throw new NotImplementedException();
    }
}
