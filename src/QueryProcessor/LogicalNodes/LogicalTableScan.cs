public class LogicalTableScan : LogicalNode {
    public string TableName { get; }
    public LogicalTableScan(string tableName) { Type = LogicalNodeType.Scan; TableName = tableName; }
}
