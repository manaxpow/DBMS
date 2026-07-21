using FluentAssertions;

public class CreateTableCommandTests
{
    [Fact]
    public void Execute_WhenTableDoesNotExist_ShouldAddTableAndReturnSuccess()
    {
        // Arrange
        var schema = new Schema("TestSchema");
        var command = new CreateTableCommand(schema, "NewTable");

        // Act
        var result = command.Execute();

        // Assert
        result.Should().Be(DDLResult.Success);
    }

    [Fact]
    public void Execute_WhenTableAlreadyExists_ShouldThrowTableAlreadyExistsException()
    {
        // Arrange
        var schema = new Schema("TestSchema");
        schema.RegisterObject(new Table("ExistingTable"));
        var command = new CreateTableCommand(schema, "ExistingTable");

        // Act
        Action action = () => command.Execute();

        // Assert
        action.Should().Throw<TableAlreadyExistsException>();
    }
}