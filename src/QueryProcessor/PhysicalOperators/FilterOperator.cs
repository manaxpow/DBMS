using System;

public class FilterOperator : PhysicalOperator {
    public PhysicalOperator Child { get; }
    public Func<Row, bool> Predicate { get; }
    
    public FilterOperator(PhysicalOperator child, Func<Row, bool> predicate) {
        throw new NotImplementedException();
    }

    public override void Open() => throw new NotImplementedException();
    public override bool Next() => throw new NotImplementedException();
    public override Row GetCurrent() => throw new NotImplementedException();
    public override void Close() => throw new NotImplementedException();
}
