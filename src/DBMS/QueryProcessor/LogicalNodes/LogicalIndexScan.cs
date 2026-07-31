public class LogicalIndexScan(string tableName, string indexName)
    : LogicalNode
{
    public string TableName { get; } = tableName;

    public string IndexName { get; } = indexName;
}
