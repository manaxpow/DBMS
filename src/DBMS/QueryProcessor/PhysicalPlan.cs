using System;

public enum PhysicalOperatorType
{
    IndexScan,
    TableScan,
    NestedLoopJoin,
}

public class PhysicalPlan
{
    public int Cost { get; set; }

    public PhysicalOperatorType OperatorType { get; set; }

    public void Build()
    {
        throw new NotImplementedException();
    }

    public void CalculateCost()
    {
        throw new NotImplementedException();
    }

    public void Validate()
    {
        throw new NotImplementedException();
    }

    public bool EquivalentTo(LogicalPlan plan)
    {
        throw new NotImplementedException();
    }
}
