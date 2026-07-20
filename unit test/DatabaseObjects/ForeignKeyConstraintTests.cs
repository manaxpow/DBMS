using System;
using FluentAssertions;
using Xunit;

public class ForeignKeyConstraintTests
{
    private (Schema, Table, Table) CreateSchema()
    {
        var schema = new Schema("TestSchema");

        var parentTable = new Table("ParentTable");
        parentTable.AddColumn(new Column("Id", typeof(int)));
        schema.AddTable(parentTable);

        var childTable = new Table("ChildTable");
        childTable.AddColumn(new Column("ParentId", typeof(int)));
        schema.AddTable(childTable);

        return (schema, parentTable, childTable);
    }

    [Trait("Category", "Important")]
    [Fact]
    public void Validate_WhenReferencedValueExists_ShouldReturnTrue()
    {
        // Arrange
        var (schema, parentTable, childTable) = CreateSchema();
        parentTable.InsertRow(new Row(parentTable, new List<object> { 1 }));

        var constraint = new ForeignKeyConstraint("FK_Parent", "ParentId", "ParentTable", "Id");

        var candidateRow = new Row(childTable, new List<object> { 1 });
        var context = new ConstraintContext(candidateRow, childTable, schema);

        // Act
        var result = constraint.Validate(context);

        // Assert
        result.Should().BeTrue();
    }

    [Trait("Category", "Important")]
    [Fact]
    public void Validate_WhenReferencedValueDoesNotExist_ShouldReturnFalse()
    {
        // Arrange
        var (schema, parentTable, childTable) = CreateSchema(); // No rows inserted into parentTable

        var constraint = new ForeignKeyConstraint("FK_Parent", "ParentId", "ParentTable", "Id");

        var candidateRow = new Row(childTable, new List<object> { 99 });
        var context = new ConstraintContext(candidateRow, childTable, schema);

        // Act
        var result = constraint.Validate(context);

        // Assert
        result.Should().BeFalse();
    }

    [Trait("Category", "Important")]
    [Fact]
    public void Validate_WhenForeignKeyValueIsNull_ShouldSkipReferenceCheck()
    {
        // Arrange
        var (schema, parentTable, childTable) = CreateSchema();

        var constraint = new ForeignKeyConstraint("FK_Parent", "ParentId", "ParentTable", "Id") { IsNullable = true };

        var candidateRow = new Row(childTable, new List<object> { null });
        var context = new ConstraintContext(candidateRow, childTable, schema);

        // Act
        var result = constraint.Validate(context);

        // Assert
        result.Should().BeTrue();
    }

    [Trait("Category", "Important")]
    [Fact]
    public void Validate_WhenReferencedTableDoesNotExist_ShouldThrow()
    {

        // Arrange
        var schema = new Schema("TestSchema");
        var childTable = new Table("ChildTable");
        childTable.AddColumn(new Column("ParentId", typeof(int)));
        schema.AddTable(childTable);

        var constraint = new ForeignKeyConstraint("FK_Parent", "ParentId", "NonExistentTable", "Id");

        var candidateRow = new Row(childTable, new List<object> { 1 });
        var context = new ConstraintContext(candidateRow, childTable, schema);

        // Act
        Action act = () => constraint.Validate(context);

        // Assert
        act.Should().Throw<TableNotFoundException>();
    }

    [Trait("Category", "Important")]
    [Fact]
    public void Validate_WhenReferencedColumnDoesNotExist_ShouldThrow()
    {

        // Arrange
        var (schema, parentTable, childTable) = CreateSchema();

        var constraint = new ForeignKeyConstraint("FK_Parent", "ParentId", "ParentTable", "NonExistentColumn");

        var candidateRow = new Row(childTable, new List<object> { 1 });
        var context = new ConstraintContext(candidateRow, childTable, schema);

        // Act
        Action act = () => constraint.Validate(context);

        // Assert
        act.Should().Throw<ColumnNotFoundException>();
    }
}
