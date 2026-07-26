using System.Collections.Generic;

public abstract class CompositePhysicalOperator : PhysicalOperator {
    protected List<PhysicalOperator> _children = new List<PhysicalOperator>();

    public void AddChild(PhysicalOperator child) {
        _children.Add(child);
    }

    public void RemoveChild(PhysicalOperator child) {
        _children.Remove(child);
    }

    public IReadOnlyList<PhysicalOperator> GetChildren() {
        return _children.AsReadOnly();
    }
}
