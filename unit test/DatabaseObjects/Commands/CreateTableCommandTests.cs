using FluentAssertions;

public class CreateTableCommandTests
{
    [Fact]
    public void Execute_WhenTableDoesNotExist_ShouldAddTableAndReturnSuccess()
    {
        var schema = new Schema("TestSchema");
        var command = new CreateTableCommand(schema, "NewTable", null);

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
        var command = new CreateTableCommand(schema, "ExistingTable", null);

        // Act
        Action action = () => command.Execute();

        // Assert
        action.Should().Throw<TableAlreadyExistsException>();
    }
}