public class LogicalTableScan(string tableName)
    : LogicalNode
{
    public LogicalTableScan()
        : this(string.Empty)
    {
    } // In case it's needed

    public string TableName { get; } = tableName;
}
