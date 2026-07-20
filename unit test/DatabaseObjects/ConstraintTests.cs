using System;
using FluentAssertions;
using Xunit;

public class TestConstraint : Constraint
{
    public bool CheckCalled { get; private set; }
    public bool CheckReturnValue { get; set; } = true;

    public TestConstraint(string name) : base(name) { }

    protected override bool Check(ConstraintContext context)
    {
        CheckCalled = true;
        return CheckReturnValue;
    }
}

public class ConstraintTests
{
    private ConstraintContext CreateDummyContext()
    {
        var schema = new Schema("TestSchema");
        var table = new Table("TestTable");
        var candidateRow = new Row(table, new List<object>());
        return new ConstraintContext(candidateRow, table, schema);
    }

    [Trait("Category", "Important")]
    [Fact]
    public void Validate_WhenConstraintIsEnabled_ShouldCallCheck()
    {
        // Arrange
        var constraint = new TestConstraint("Test");
        var context = CreateDummyContext();
        constraint.Enable();

        // Act
        constraint.Validate(context);

        // Assert
        constraint.CheckCalled.Should().BeTrue();
    }

    [Trait("Category", "Important")]
    [Fact]
    public void Validate_WhenCheckReturnsTrue_ShouldReturnTrue()
    {
        // Arrange
        var constraint = new TestConstraint("Test") { CheckReturnValue = true };
        var context = CreateDummyContext();

        // Act
        var result = constraint.Validate(context);

        // Assert
        result.Should().BeTrue();
    }

    [Trait("Category", "Important")]
    [Fact]
    public void Validate_WhenCheckReturnsFalse_ShouldReturnFalse()
    {
        // Arrange
        var constraint = new TestConstraint("Test") { CheckReturnValue = false };
        var context = CreateDummyContext();

        // Act
        var result = constraint.Validate(context);

        // Assert
        result.Should().BeFalse();
    }

    [Trait("Category", "Important")]
    [Fact]
    public void Validate_WhenConstraintIsDisabled_ShouldSkipCheck()
    {
        // Arrange
        var constraint = new TestConstraint("Test");
        var context = CreateDummyContext();

        // Act
        constraint.Disable();
        var result = constraint.Validate(context);

        // Assert
        constraint.CheckCalled.Should().BeFalse();
        result.Should().BeTrue();
    }

    [Trait("Category", "Important")]
    [Fact]
    public void Disable_WhenConstraintIsEnabled_ShouldDisable()
    {
        // Arrange
        var constraint = new TestConstraint("Test");
        constraint.Enable();

        // Act
        constraint.Disable();

        // Assert
        constraint.IsEnabled.Should().BeFalse();
    }

    [Trait("Category", "Important")]
    [Fact]
    public void Enable_WhenConstraintIsDisabled_ShouldEnable()
    {
        // Arrange
        var constraint = new TestConstraint("Test");
        constraint.Disable();

        // Act
        constraint.Enable();

        // Assert
        constraint.IsEnabled.Should().BeTrue();
    }
}
