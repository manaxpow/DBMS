public class TableScanOperator : PhysicalOperator {
    public string TableName { get; }
    public TableScanOperator(string tableName) { TableName = tableName; }
    public override void Open() { }
    public override bool Next() { return false; }
    public override void Close() { }
}
