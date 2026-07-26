using Xunit;

public class CompositePhysicalOperatorTests
{
    private class TestCompositeOperator : CompositePhysicalOperator
    {
        public override void Open() { }
        public override bool Next() { return false; }
        public override void Close() { }
    }

    private class TestLeafOperator : PhysicalOperator
    {
        public override void Open() { }
        public override bool Next() { return false; }
        public override void Close() { }
    }

    [Fact]
    public void AddChild_ShouldAddChildToOperator()
    {
        var composite = new TestCompositeOperator();
        var child = new TestLeafOperator();
        
        composite.AddChild(child);

        var children = composite.GetChildren();
        Assert.Single(children);
        Assert.Equal(child, children[0]);
    }

    [Fact]
    public void RemoveChild_ShouldRemoveChildFromOperator()
    {
        var composite = new TestCompositeOperator();
        var child = new TestLeafOperator();
        composite.AddChild(child);
        
        composite.RemoveChild(child);

        var children = composite.GetChildren();
        Assert.Empty(children);
    }
}
