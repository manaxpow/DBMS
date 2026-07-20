using System;
using System.Collections.Generic;
using FluentAssertions;
using Xunit;

public class PrimaryKeyConstraintTests
{
    private (Table, Schema) CreateTable()
    {
        var schema = new Schema("TestSchema");
        var table = new Table("TestTable");
        table.AddColumn(new Column("Id", typeof(int)));
        schema.AddTable(table);
        return (table, schema);
    }

    [Trait("Category", "Important")]
    [Fact]
    public void Validate_WhenKeyIsUniqueAndNotNull_ShouldReturnTrue()
    {
        // Arrange
        var (table, schema) = CreateTable();
        table.InsertRow(new Row(table, new List<object> { 1 }));

        var constraint = new PrimaryKeyConstraint("PK_Id", new[] { "Id" });
        var candidateRow = new Row(table, new List<object> { 2 });
        var context = new ConstraintContext(candidateRow, table, schema);

        // Act
        var result = constraint.Validate(context);

        // Assert
        result.Should().BeTrue();
    }

    [Trait("Category", "Important")]
    [Fact]
    public void Validate_WhenKeyContainsNull_ShouldReturnFalse()
    {
        // Arrange
        var (table, schema) = CreateTable();

        var constraint = new PrimaryKeyConstraint("PK_Id", new[] { "Id" });
        var candidateRow = new Row(table, new List<object> { null });
        var context = new ConstraintContext(candidateRow, table, schema);

        // Act
        var result = constraint.Validate(context);

        // Assert
        result.Should().BeFalse();
    }

    [Trait("Category", "Important")]
    [Fact]
    public void Validate_WhenDuplicateKeyExists_ShouldReturnFalse()
    {
        // Arrange
        var (table, schema) = CreateTable();
        table.InsertRow(new Row(table, new List<object> { 1 }));

        var constraint = new PrimaryKeyConstraint("PK_Id", new[] { "Id" });
        var candidateRow = new Row(table, new List<object> { 1 });
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
        var existingRow = new Row(table, new List<object> { 1 });
        table.InsertRow(existingRow);

        var constraint = new PrimaryKeyConstraint("PK_Id", new[] { "Id" });
        var candidateRow = new Row(table, new List<object> { 1 });
        var context = new ConstraintContext(candidateRow, table, schema, existingRow);

        // Act
        var result = constraint.Validate(context);

        // Assert
        result.Should().BeTrue();
    }
}



