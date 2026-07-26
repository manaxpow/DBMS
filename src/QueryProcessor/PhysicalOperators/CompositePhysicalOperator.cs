using System.Collections.Generic;

public abstract class CompositePhysicalOperator : PhysicalOperator {
    protected List<PhysicalOperator> _children = new List<PhysicalOperator>();

    public void AddChild(PhysicalOperator child) => throw new System.NotImplementedException();
    public void RemoveChild(PhysicalOperator child) => throw new System.NotImplementedException();
    public IReadOnlyList<PhysicalOperator> GetChildren() => throw new System.NotImplementedException();
}
