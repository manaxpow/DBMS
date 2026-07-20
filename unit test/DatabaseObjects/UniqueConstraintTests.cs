using FluentAssertions;

public class UniqueConstraintTests
{
    private (Table, Schema) CreateTable()
    {
        var schema = new Schema("TestSchema");
        var table = new Table("TestTable");
        table.AddColumn(new Column("Id", typeof(int)));
        table.AddColumn(new Column("Name", typeof(string)));
        schema.AddTable(table);
        return (table, schema);
    }

    [Trait("Category", "Important")]
    [Fact]
    public void Validate_WhenKeyIsUnique_ShouldReturnTrue()
    {
        // Arrange
        var (table, schema) = CreateTable();
        table.InsertRow(new Row(table, new List<object> { 1, "Alice" }));

        var constraint = new UniqueConstraint("UC_Id", new[] { "Id" });
        var candidateRow = new Row(table, new List<object> { 2, "Bob" });
        var context = new ConstraintContext(candidateRow, table, schema);

        // Act
        var result = constraint.Validate(context);

        // Assert
        result.Should().BeTrue();
    }

    [Trait("Category", "Important")]
    [Fact]
    public void Validate_WhenDuplicateKeyExists_ShouldReturnFalse()
    {
        // Arrange
        var (table, schema) = CreateTable();
        table.InsertRow(new Row(table, new List<object> { 1, "Alice" }));

        var constraint = new UniqueConstraint("UC_Id", new[] { "Id" });
        var candidateRow = new Row(table, new List<object> { 1, "Bob" });
        var context = new ConstraintContext(candidateRow, table, schema);

        // Act
        var result = constraint.Validate(context);

        // Assert
        result.Should().BeFalse();
    }

    [Trait("Category", "Important")]
    [Fact]
    public void Validate_WhenUpdatingSameRow_ShouldIgnoreExistingRow()
    {
        // Arrange
        var (table, schema) = CreateTable();
        var existingRow = new Row(table, new List<object> { 1, "Alice" });
        table.InsertRow(existingRow);

        var constraint = new UniqueConstraint("UC_Id", new[] { "Id" });
        var candidateRow = new Row(table, new List<object> { 1, "Alice_Updated" });
        var context = new ConstraintContext(candidateRow, table, schema, existingRow);

        // Act
        var result = constraint.Validate(context);

        // Assert
        result.Should().BeTrue();
    }

    [Trait("Category", "Important")]
    [Fact]
    public void Validate_WhenCompositeKeyAlreadyExists_ShouldReturnFalse()
    {
        // Arrange
        var (table, schema) = CreateTable();
        table.InsertRow(new Row(table, new List<object> { 1, "Alice" }));

        var constraint = new UniqueConstraint("UC_Composite", new[] { "Id", "Name" });

        // Match only one part of the composite key
        var candidateRow1 = new Row(table, new List<object> { 1, "Bob" });
        var context1 = new ConstraintContext(candidateRow1, table, schema);

        // Match the full composite key
        var candidateRow2 = new Row(table, new List<object> { 1, "Alice" });
        var context2 = new ConstraintContext(candidateRow2, table, schema);

        // Act
        var result1 = constraint.Validate(context1);
        var result2 = constraint.Validate(context2);

        // Assert
        result1.Should().BeTrue();
        result2.Should().BeFalse();
    }
}
