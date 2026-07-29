public class TableScanOperator(string tableName, IEnumerable<Row> tableRows)
    : PhysicalOperator
{
    // private IEnumerator<Row> enumerator = null!;
    private IEnumerable<Row> tableRows = tableRows;

    public string TableName { get; } = tableName;

    public override void Open() => throw new NotImplementedException();

    public override bool Next() => throw new NotImplementedException();

    public override Row GetCurrent() => throw new NotImplementedException();

    public override void Close() => throw new NotImplementedException();
}
