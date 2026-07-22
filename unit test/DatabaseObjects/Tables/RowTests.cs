using FluentAssertions;

public class RowTests
{
    private readonly Row _row;

    public RowTests()
    {
        var table = new Table("TestTable");
        var column1 = new Column("Id", typeof(int), isNullable: false);
        var column2 = new Column("Name", typeof(string), isNullable: true);
        table.AddColumn(column1);
        table.AddColumn(column2);

        var values = new List<object> { 1, "Test" };
        _row = new Row(table, values);
    }
    [Trait("Category", "Important")]
    [Fact]
    public void GetValue_WhenColumnExists_ShouldReturnValue()
    {
        // Arrange
        var columnName = "Name";

        // Act
        var value = _row.GetValue(columnName);

        // Assert
        value.Should().Be("Test");
    }

    [Trait("Category", "Important")]
    [Fact]
    public void GetValue_WhenColumnDoesNotExist_ShouldThrow()
    {
        // Arrange
        var columnName = "NonExistentColumn";

        // Act
        Action act = () => _row.GetValue(columnName);

        // Assert
        act.Should().Throw<ColumnNotFoundException>();
    }

    [Trait("Category", "Important")]
    [Fact]
    public void SetValue_WhenValueIsValid_ShouldUpdateValue()
    {
        // Arrange
        var columnName = "Name";
        var newValue = "Updated";

        // Act
        _row.SetValue(columnName, newValue);

        // Assert
        _row.GetValue(columnName).Should().Be(newValue);
    }

    [Trait("Category", "Important")]
    [Fact]
    public void SetValue_WhenTypeDoesNotMatch_ShouldThrow()
    {
        // Arrange
        var columnName = "Id";
        var newValue = "InvalidType"; // This should be an int, not a string

        // Act
        Action act = () => _row.SetValue(columnName, newValue);

        // Assert
        act.Should().Throw<InvalidColumnValueException>();
    }



    [Trait("Category", "Important")]
    [Fact]
    public void SetValue_WhenColumnDoesNotExist_ShouldThrow()
    {
        // Arrange
        var columnName = "NonExistentColumn";
        var newValue = "SomeValue";

        // Act
        Action act = () => _row.SetValue(columnName, newValue);

        // Assert
        act.Should().Throw<ColumnNotFoundException>();
    }

    [Fact]
    public void SetValue_WhenNullIsAllowed_ShouldUpdateValue()
    {
        // Arrange
        var columnName = "Name";
        object newValue = null; // Null value for a nullable column

        // Act
        _row.SetValue(columnName, newValue);

        // Assert
        _row.GetValue(columnName).Should().BeNull();
    }

    [Fact]
    public void SetValue_WhenNullIsNotAllowed_ShouldThrow()
    {
        // Arrange
        var columnName = "Id";
        object newValue = null; // Null value for a non-nullable column

        // Act
        Action act = () => _row.SetValue(columnName, newValue);

        // Assert
        act.Should().Throw<InvalidColumnValueException>();
    }

    [Trait("Category", "Important")]
    [Fact]
    public void SetValue_WhenValidationFails_ShouldPreserveExistingValue()
    {
        // Arrange
        var columnName = "Name";
        var originalValue = "John";
        object invalidValue = null;
        _row.SetValue(columnName, originalValue);

        // Act
        Action act = () => _row.SetValue(columnName, invalidValue);

        // Assert
        act.Should().Throw<InvalidColumnValueException>();
        _row.GetValue(columnName).Should().Be(originalValue);
    }
}


