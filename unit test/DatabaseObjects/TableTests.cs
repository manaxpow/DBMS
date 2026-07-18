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

    [Fact]
    public void AddColumn_WhenColumnIsValid_ShouldAddColumn()
    {
        throw new NotImplementedException();
    }

    [Fact]
    public void AddColumn_WhenColumnIsNull_ShouldThrow()
    {
        throw new NotImplementedException();
    }

    [Fact]
    public void InsertRow_WhenRowIsNull_ShouldThrow()
    {
        throw new NotImplementedException();
    }

    [Fact]
    public void InsertRow_WhenValueCountDoesNotMatch_ShouldThrow()
    {
        throw new NotImplementedException();
    }

    [Fact]
    public void InsertRow_WhenValueTypeDoesNotMatch_ShouldThrow()
    {
        throw new NotImplementedException();
    }

    [Fact]
    public void InsertRow_WhenNullValueIsAllowed_ShouldInsertRow()
    {
        throw new NotImplementedException();
    }

    [Fact]
    public void InsertRow_WhenNullValueIsNotAllowed_ShouldThrow()
    {
        throw new NotImplementedException();
    }

    [Fact]
    public void DeleteRow_WhenRowExists_ShouldRemoveRow()
    {
        throw new NotImplementedException();
    }

    [Fact]
    public void DeleteRow_WhenRowDoesNotExist_ShouldReturnFalse()
    {
        throw new NotImplementedException();
    }

    [Fact]
    public void DropColumn_WhenColumnExists_ShouldRemoveColumn()
    {
        throw new NotImplementedException();
    }

    [Fact]
    public void DropColumn_WhenColumnDoesNotExist_ShouldThrow()
    {
        throw new NotImplementedException();
    }

    [Fact]
    public void DropColumn_WhenColumnIsReferencedByConstraint_ShouldThrow()
    {
        throw new NotImplementedException();
    }

    [Fact]
    public void DropColumn_WhenRowsExist_ShouldRemoveCorrespondingValues()
    {
        throw new NotImplementedException();
    }

    [Fact]
    public void AlterColumn_WhenColumnExists_ShouldUpdateDefinition()
    {
        throw new NotImplementedException();
    }

    [Fact]
    public void AlterColumn_WhenColumnDoesNotExist_ShouldThrow()
    {
        throw new NotImplementedException();
    }
}
