public class DropTableCommand : IDDLCommand
{
    private readonly Schema _schema;
    private readonly string _tableName;

    public DropTableCommand(Schema schema, string tableName)
    {
        this._schema = schema;
        this._tableName = tableName;
    }

    public DDLResult Execute()
    {
        throw new NotImplementedException();
    }
}
