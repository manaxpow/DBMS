using System.Collections.Generic;

public abstract class CompositePhysicalOperator : PhysicalOperator
{
    private List<PhysicalOperator> _children = new List<PhysicalOperator>();

    protected List<PhysicalOperator> Children => this._children;

    public void AddChild(PhysicalOperator child) => throw new System.NotImplementedException();

    public void RemoveChild(PhysicalOperator child) => throw new System.NotImplementedException();

    public IReadOnlyList<PhysicalOperator> GetChildren() => throw new System.NotImplementedException();
}
