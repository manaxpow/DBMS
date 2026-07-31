using System;
using FluentAssertions;
using Xunit;

public class CheckConstraintTests
{
    private ConstraintContext CreateDummyContext(params object[] values)
    {
        var schema = new Schema("TestSchema");
        var table = new Table("TestTable");
        var column = new Column("Value", DataTypeFactory.Create("INT"));
        table.AddColumn(column);

        var candidateRow = new Row(table, new List<object>(values));
        return new ConstraintContext(candidateRow, table, schema);
    }

    [Trait("Category", "Important")]
    [Fact]
    public void Validate_WhenPredicateReturnsTrue_ShouldReturnTrue()
    {
        // Arrange
        var constraint = new CheckConstraint("Test", row => true);
        var context = CreateDummyContext(5);

        // Act
        var result = constraint.Validate(context);

        // Assert
        result.Should().BeTrue();
    }

    [Trait("Category", "Important")]
    [Fact]
    public void Validate_WhenPredicateReturnsFalse_ShouldReturnFalse()
    {
        // Arrange
        var constraint = new CheckConstraint("Test", row => false);
        var context = CreateDummyContext(5);

        // Act
        var result = constraint.Validate(context);

        // Assert
        result.Should().BeFalse();
    }

    [Trait("Category", "Important")]
    [Fact]
    public void Validate_WhenPredicateUsesMultipleColumns_ShouldEvaluateCandidateRow()
    {
        // Arrange
        var schema = new Schema("TestSchema");
        var table = new Table("TestTable");
        table.AddColumn(new Column("Min", DataTypeFactory.Create("INT")));
        table.AddColumn(new Column("Max", DataTypeFactory.Create("INT")));

        var validRow = new Row(table, new List<object> { 10, 20 });
        var invalidRow = new Row(table, new List<object> { 30, 20 });

        var constraint = new CheckConstraint("Test", row => (int)row.GetValue("Min") < (int)row.GetValue("Max"));

        var validContext = new ConstraintContext(validRow, table, schema);
        var invalidContext = new ConstraintContext(invalidRow, table, schema);

        // Act
        var validResult = constraint.Validate(validContext);
        var invalidResult = constraint.Validate(invalidContext);

        // Assert
        validResult.Should().BeTrue();
        invalidResult.Should().BeFalse();
    }
}




