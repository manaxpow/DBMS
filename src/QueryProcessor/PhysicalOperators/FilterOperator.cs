using System;

public class FilterOperator(PhysicalOperator child, Func<Row, bool> predicate)
    : PhysicalOperator
{
    public PhysicalOperator Child { get; } = child;

    public Func<Row, bool> Predicate { get; } = predicate;

    public override void Open() => throw new NotImplementedException();

    public override bool Next() => throw new NotImplementedException();

    public override Row GetCurrent() => throw new NotImplementedException();

    public override void Close() => throw new NotImplementedException();
}
