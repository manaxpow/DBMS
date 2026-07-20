using FluentAssertions;
public class TableTests
{
    private Table _table;
    public TableTests()
    {
        _table = new Table("TestTable");
    }
    [Fact]
    public void AddColumn_WhenColumnIsValid_ShouldAddColumn()
    {
        // Arrange
        var column = new Column("Id", typeof(int));

        // Act
        _table.AddColumn(column);

        // Assert
        _table.ContainsColumn(column.Name).Should().Be(true);
        _table.Columns.Should().Contain(column);
    }

    [Fact]
    public void AddColumn_WhenColumnIsNull_ShouldThrow()
    {
        // Act
        Action act = () => _table.AddColumn(null!);

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void AddColumn_WhenNameAlreadyExists_ShouldThrow()
    {
        // Arrange
        var column1 = new Column("Id", typeof(int));
        _table.AddColumn(column1);

        // Duplicate name
        var column2 = new Column("Id", typeof(string));

        // Act
        Action act = () => _table.AddColumn(column2);

        // Assert
        _table.ContainsColumn(column1.Name).Should().Be(true);
        act.Should().Throw<InvalidOperationException>();
    }

    // Internal
    [Fact]
    public void InsertRow_WhenRowIsValid_ShouldInsertRow()
    {
        // Arrange
        var row = new Row(_table, new List<object> { 1, "Test" });

        // Act
        _table.InsertRow(row);

        // Assert
        _table.ContainsRow(row).Should().Be(true);
        _table.Rows.Should().Contain(row);
    }

    [Fact]
    public void InsertRow_WhenRowIsNull_ShouldThrow()
    {
        // Act
        Action act = () => _table.InsertRow(null!);

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void InsertRow_WhenValueCountDoesNotMatch_ShouldThrow()
    {
        // Arrange
        var column1 = new Column("Id", typeof(int));
        var column2 = new Column("Name", typeof(string));
        _table.AddColumn(column1);
        _table.AddColumn(column2);

        // Create a row with only one value instead of two
        var row = new Row(_table, new List<object> { 1 });

        // Act
        Action act = () => _table.InsertRow(row);

        // Assert
        _table.Columns.Count.Should().Be(2);
        act.Should().Throw<RowSchemaMismatchException>();
    }

    [Fact]
    public void InsertRow_WhenValueTypeDoesNotMatch_ShouldThrow()
    {
        // Arrange
        var column1 = new Column("Id", typeof(int));
        var column2 = new Column("Name", typeof(string));
        _table.AddColumn(column1);
        _table.AddColumn(column2);

        // Create a row with a value type that does not match the column type
        var row = new Row(_table, new List<object> { 1, 123 }); // 123 is not a string

        // Act
        Action act = () => _table.InsertRow(row);

        // Assert
        _table.Columns.Count.Should().Be(2);
        act.Should().Throw<RowSchemaMismatchException>();
    }

    [Fact]
    public void InsertRow_WhenNullValueIsAllowed_ShouldInsertRow()
    {
        // Arrange
        var column1 = new Column("Id", typeof(int));
        var column2 = new Column("Name", typeof(string), isNullable: true);
        _table.AddColumn(column1);
        _table.AddColumn(column2);

        // Null name is allowed
        var row = new Row(_table, new List<object> { 1, null! });

        // Act
        _table.InsertRow(row);

        // Assert
        _table.ContainsRow(row).Should().Be(true);
        _table.Rows.Should().Contain(row);
    }

    [Fact]
    public void InsertRow_WhenNullValueIsNotAllowed_ShouldThrow()
    {
        // Arrange
        var column1 = new Column("Id", typeof(int));
        var column2 = new Column("Name", typeof(string), isNullable: false);
        _table.AddColumn(column1);
        _table.AddColumn(column2);

        // Create a row with a null value for the "Name" column
        var row = new Row(_table, new List<object> { 1, null! });

        // Act
        Action act = () => _table.InsertRow(row);

        // Assert
        _table.Columns.Count.Should().Be(2);
        act.Should().Throw<RowSchemaMismatchException>();
    }

    [Fact]
    public void DeleteRow_WhenRowExists_ShouldRemoveRow()
    {
        // Arrange
        var column1 = new Column("Id", typeof(int));
        _table.AddColumn(column1);
        var row = new Row(_table, new List<object> { 1 });
        _table.InsertRow(row);
        var isRowAddSuccess = _table.ContainsRow(row);

        // Act
        _table.DeleteRow(row);

        // Assert
        isRowAddSuccess.Should().Be(true);
        _table.ContainsRow(row).Should().Be(false);
    }

    [Fact]
    public void DeleteRow_WhenRowDoesNotExist_ShouldReturnFalse()
    {
        // Arrange
        var row = new Row(_table, new List<object> { 1 });

        // Act
        var isDeleted = _table.DeleteRow(row);

        // Assert
        isDeleted.Should().BeFalse();
    }

    [Fact]
    public void DropColumn_WhenColumnExists_ShouldRemoveColumn()
    {
        // Arrange
        var column = new Column("Id", typeof(int));
        _table.AddColumn(column);
        var isColumnAddSuccess = _table.ContainsColumn(column.Name);

        // Act
        _table.DropColumn(column.Name);

        // Assert
        isColumnAddSuccess.Should().Be(true);
        _table.Columns.Should().NotContain(column);
    }

    [Fact]
    public void DropColumn_WhenColumnDoesNotExist_ShouldThrow()
    {
        // Act
        Action act = () => _table.DropColumn("NonExistentColumn");

        // Assert
        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void DropColumn_WhenColumnIsReferencedByConstraint_ShouldThrow()
    {
        // Arrange
        var referencedTable = new Table("ReferencedTable");
        var column = new Column("Id", typeof(int));
        _table.AddColumn(column);
        var constraint = new ForeignKey("FK_TestTable_Id", "Id", referencedTableName: "ReferencedTable", referencedColumnName: "Id");
        _table.AddConstraint(constraint);

        // Act
        Action act = () => _table.DropColumn(column.Name);

        // Assert
        act.Should().Throw<ColumnReferencedException>();
    }

    [Fact]
    public void DropColumn_WhenRowsExist_ShouldRemoveCorrespondingValues()
    {
        // Arrange
        var column1 = new Column("Id", typeof(int));
        var column2 = new Column("Name", typeof(string));
        _table.AddColumn(column1);
        _table.AddColumn(column2);
        var row1 = new Row(_table, new List<object> { 1, "Alice" });
        var row2 = new Row(_table, new List<object> { 2, "Bob" });
        _table.InsertRow(row1);
        _table.InsertRow(row2);

        // Act
        _table.DropColumn(column2.Name);

        // Assert
        _table.Columns.Should().NotContain(column2);
        foreach (var row in _table.Rows)
        {
            row.Values.Should().NotContain(column2.Name);
        }
    }

    [Fact]
    public void AlterColumn_WhenColumnExists_ShouldUpdateDefinition()
    {
        // Arrange
        var column = new Column("Id", typeof(int));
        _table.AddColumn(column);
        var newColumnDefinition = new Column("Id", typeof(long));

        // Act
        _table.AlterColumn(column.Name, newColumnDefinition);

        // Assert
        var updatedColumn = _table.GetColumn(column.Name);
        updatedColumn.DataType.Should().Be(typeof(long));
    }

    [Fact]
    public void AlterColumn_WhenColumnDoesNotExist_ShouldThrow()
    {
        // Act
        var column = new Column("NonExistentColumn", typeof(string));
        Action act = () => _table.AlterColumn("NonExistentColumn", column);

        // Assert
        act.Should().Throw<ColumnNotFoundException>();
    }
}
