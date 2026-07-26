using Xunit;

public class CompositePhysicalOperatorTests
{
    private class TestCompositeOperator : CompositePhysicalOperator
    {
        public override void Open() { }
        public override bool Next() => false;
        public override Row GetCurrent() => null!;
        public override void Close() { }
    }

    private class TestLeafOperator : PhysicalOperator
    {
        public override void Open() { }
        public override bool Next() => false;
        public override Row GetCurrent() => null!;
        public override void Close() { }
    }

    [Fact]
    public void AddChild_ShouldAddChildToOperator()
    {
        throw new System.NotImplementedException();
        // Arrange
        var composite = new TestCompositeOperator();
        var child = new TestLeafOperator();
        
        // Act
        composite.AddChild(child);

        // Assert
        var children = composite.GetChildren();
        Assert.Single(children);
        Assert.Equal(child, children[0]);
    }

    [Fact]
    public void RemoveChild_ShouldRemoveChildFromOperator()
    {
        throw new System.NotImplementedException();
        // Arrange
        var composite = new TestCompositeOperator();
        var child = new TestLeafOperator();
        composite.AddChild(child);
        
        // Act
        composite.RemoveChild(child);

        // Assert
        var children = composite.GetChildren();
        Assert.Empty(children);
    }
}

