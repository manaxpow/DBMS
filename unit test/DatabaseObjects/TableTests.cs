using System;
using Xunit;

public class TableTests
{
    private Table _table;
    public TableTests()
    {
        _table = new Table("TestTable");
    }
    [Fact]
    public void InsertRow_WhenRowIsValid_ShouldInsertRow()
    {
        // Arrange
        var row = new Row();
        row.Values = new object[] { 1, "Test" };

        // Act
        _table.InsertRow(row);

        // Assert
        Assert.True(_table.ContainsRow(row));
        Assert.Contains(row, _table.Rows);
    }

    [Fact]
    public void InsertRow_WhenSchemaDoesNotMatch_ShouldThrow()
    {
        // Arrange
        // Assume the table has 2 columns defined
        var column1 = new Column { Name = "Id", Type = "int" };
        var column2 = new Column { Name = "Name", Type = "string" };
        _table.Columns.Add(column1);
        _table.Columns.Add(column2);

        // Create row with 3 values, which does not match the schema of 2 columns
        var row = new Row();
        row.Values = new object[] { 1, "Test", 3.14 };

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => _table.InsertRow(row));
        Assert.False(_table.ContainsRow(row));
    }

    [Fact]
    public void AddColumn_WhenNameAlreadyExists_ShouldThrow()
    {
        // Arrange
        var column1 = new Column { Name = "Id", Type = "int" };
        _table.AddColumn(column1);

        // Duplicate name
        var column2 = new Column { Name = "Id", Type = "string" };

        // Act & Assert
        Assert.True(_table.ContainsColumn(column1.Name));
        Assert.Throws<InvalidOperationException>(() => _table.AddColumn(column2));
    }
}
