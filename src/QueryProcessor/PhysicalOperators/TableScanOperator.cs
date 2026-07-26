using System.Collections.Generic;

public class TableScanOperator : PhysicalOperator {
    public string TableName { get; }
    private IEnumerator<Row> _enumerator;
    private IEnumerable<Row> _tableRows;

    public TableScanOperator(string tableName, IEnumerable<Row> tableRows) { 
        TableName = tableName; 
        _tableRows = tableRows;
    }

    public override void Open() { 
        _enumerator = _tableRows.GetEnumerator();
    }

    public override bool Next() { 
        return _enumerator.MoveNext(); 
    }

    public override Row GetCurrent() { 
        return _enumerator.Current; 
    }

    public override void Close() { 
        _enumerator?.Dispose();
    }
}
