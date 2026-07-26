using System.Collections.Generic;

public class TableScanOperator : PhysicalOperator {
    public string TableName { get; }
    private IEnumerator<Row> _enumerator;
    private IEnumerable<Row> _tableRows;

    public TableScanOperator(string tableName, IEnumerable<Row> tableRows) { 
        throw new NotImplementedException();
    }

    public override void Open() => throw new NotImplementedException();
    public override bool Next() => throw new NotImplementedException();
    public override Row GetCurrent() => throw new NotImplementedException();
    public override void Close() => throw new NotImplementedException();
}
