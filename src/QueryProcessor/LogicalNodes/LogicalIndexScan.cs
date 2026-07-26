public class LogicalIndexScan : LogicalNode {
    public string TableName { get; }
    public string IndexName { get; }
    public LogicalIndexScan(string tableName, string indexName) { Type = LogicalNodeType.IndexScan; TableName = tableName; IndexName = indexName; }
}
