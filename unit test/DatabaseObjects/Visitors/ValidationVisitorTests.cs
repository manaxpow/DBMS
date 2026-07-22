using FluentAssertions;

public class ValidationVisitorTests
{
    private readonly ValidationVisitor _visitor = new ValidationVisitor();
    public ValidationVisitorTests()
    {

    }
    [Fact]
    public void Visit_Schema_ShouldValidateSchema()
    {
        // Arrange
        var schema = new Schema("TestSchema");

        // Act
        Action action = () => _visitor.Visit(schema);

        // Assert
        action.Should().NotThrow();
    }

    [Fact]
    public void Visit_Table_ShouldValidateTable()
    {
        // Arrange
        var table = new Table("TestTable");
        // Act
        Action action = () => _visitor.Visit(table);

        // Assert
        action.Should().NotThrow();
    }

    [Fact]
    public void Visit_View_ShouldValidateView()
    {
        // Arrange
        var view = new View("TestView", "TestQuery");

        // Act
        Action action = () => _visitor.Visit(view);

        // Assert
        action.Should().NotThrow();
    }

    [Fact]
    public void Visit_StoredProcedure_ShouldValidateStoredProcedure()
    {
        // Arrange
        var storedProcedure = new StoredProcedure("TestStoredProcedure", "TestQuery");

        // Act
        Action action = () => _visitor.Visit(storedProcedure);

        // Assert
        action.Should().NotThrow();
    }
}
