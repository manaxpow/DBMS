using FluentAssertions;

public class ColumnTests
{
    Column _column;

    public ColumnTests()
    {
        _column = new Column("Id", DataTypeFactory.Create("INT"));
    }

    [Trait("Category", "Important")]
    [Fact]
    public void Create_WhenDefinitionIsValid_ShouldCreateColumn()
    {
        // Arrange
        var name = "Name";
        var type = "string";

        // Act
        var column = _column.Create(name, type, isNullable: false);

        // Assert
        column.Name.Should().Be(name);
        column.DataType.Should().Be(DataTypeFactory.Create("VARCHAR"));
        column.IsNullable.Should().BeFalse();
    }

    [Trait("Category", "Important")]
    [Fact]
    public void Create_WhenNameIsInvalid_ShouldThrow()
    {
        // Arrange
        var name = ""; // Invalid name
        var type = "string";

        // Act
        Action act = () => _column.Create(name, type, isNullable: false);

        // Assert
        act.Should().Throw<InvalidColumnNameException>();
    }

    [Fact]
    public void Create_WhenDataTypeIsNull_ShouldThrow()
    {
        // Arrange
        var name = "Name";
        string type = null;

        // Act
        Action act = () => _column.Create(name, type, isNullable: false);

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    [Trait("Category", "Important")]
    [Fact]
    public void ValidateValue_WhenTypeMatches_ShouldReturnTrue()
    {
        // Arrange
        var value = 123; // Valid type for the column

        // Act
        var isValid = _column.ValidateValue(value);

        // Assert
        isValid.Should().BeTrue();
    }

    [Trait("Category", "Important")]
    [Fact]
    public void ValidateValue_WhenTypeDoesNotMatch_ShouldReturnFalse()
    {
        // Arrange
        var value = "string"; // Invalid type for the column

        // Act
        var isValid = _column.ValidateValue(value);

        // Assert
        isValid.Should().BeFalse();
    }

    [Trait("Category", "Important")]
    [Fact]
    public void ValidateValue_WhenValueIsNullAndNullable_ShouldReturnTrue()
    {
        // Arrange
        object value = null; // Null value for a nullable column

        // Act
        var column = _column.Create("Name", "string", isNullable: true);
        var isValid = column.ValidateValue(value);

        // Assert
        isValid.Should().BeTrue();
    }

    [Trait("Category", "Important")]
    [Fact]
    public void ValidateValue_WhenValueIsNullAndNotNullable_ShouldReturnFalse()
    {
        // Arrange
        object value = null; // Null value for a non-nullable column

        // Act
        var column = _column.Create("Name", "string", isNullable: false);
        var isValid = column.ValidateValue(value);

        // Assert
        isValid.Should().BeFalse();
    }
}



