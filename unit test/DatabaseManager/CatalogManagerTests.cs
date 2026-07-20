using DBMS.Exceptions;
using FluentAssertions;
using NSubstitute;

public class CatalogManagerTests
{
    private CatalogManager _catalogManager;

    public CatalogManagerTests()
    {
        _catalogManager = new CatalogManager();
    }

    [Trait("Category", "Important")]
    [Fact]
    public void Register_WhenObjectIsValid_ShouldAddToCatalog()
    {
        // Arrange
        var database = Substitute.For<ICatalogObject>();
        database.Name.Returns("TestDatabase");

        // Act
        _catalogManager.Register(database);

        // Assert
        _catalogManager.Find<ICatalogObject>("TestDatabase").Should().NotBeNull();
    }

    [Trait("Category", "Important")]
    [Fact]
    public void Register_WhenObjectAlreadyExists_ShouldThrow()
    {
        // Arrange
        var database = Substitute.For<ICatalogObject>();
        database.Name.Returns("TestDatabase");
        _catalogManager.Register(database);

        // Act
        Action act = () => _catalogManager.Register(database);

        // Assert
        act.Should().Throw<ObjectAlreadyExistsException>();
    }


    [Trait("Category", "Important")]
    [Fact]
    public void Find_WhenObjectExists_ShouldReturnObject()
    {
        // Arrange
        var database = Substitute.For<ICatalogObject>();
        database.Name.Returns("TestDatabase");
        _catalogManager.Register(database);

        // Act
        var result = _catalogManager.Find<ICatalogObject>("TestDatabase");

        // Asssert
        result.Should().Be(database);
    }

    [Trait("Category", "Important")]
    [Fact]
    public void Remove_WhenObjectExists_ShouldRemoveObject()
    {
        // Arrange
        var database = Substitute.For<ICatalogObject>();
        database.Name.Returns("TestDatabase");
        _catalogManager.Register(database);

        // Act
        _catalogManager.Remove(database);

        // Assert
        _catalogManager.Find<ICatalogObject>("TestDatabase").Should().BeNull();
    }

    [Trait("Category", "Important")]
    [Fact]
    public void Remove_WhenObjectDoesNotExist_ShouldThrow()
    {
        // Arrange
        var database = Substitute.For<ICatalogObject>();
        database.Name.Returns("TestDatabase");

        // Act
        Action act = () => _catalogManager.Remove(database);

        // Assert
        act.Should().Throw<ObjectNotFoundException>();
    }

    [Fact]
    public void Find_WhenObjectDoesNotExist_ShouldReturnNull()
    {
        throw new NotImplementedException();
    }


    [Fact]
    public void Register_WhenObjectIsNull_ShouldThrow()
    {
        throw new NotImplementedException();
    }
}

