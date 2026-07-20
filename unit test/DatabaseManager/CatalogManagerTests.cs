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
        // Act
        _catalogManager.Register(new Database("TestDatabase"));

        // Assert
        _catalogManager.Find<Database>("TestDatabase").Should().NotBeNull();
    }

    [Trait("Category", "Important")]
    [Fact]
    public void Register_WhenObjectAlreadyExists_ShouldThrow()
    {
        throw new NotImplementedException();
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

    [Trait("Category", "Important")]
    [Fact]
    public void Find_WhenObjectExists_ShouldReturnObject()
    {
        throw new NotImplementedException();
    }

    [Trait("Category", "Important")]
    [Fact]
    public void Remove_WhenObjectExists_ShouldRemoveObject()
    {
        throw new NotImplementedException();
    }

    [Trait("Category", "Important")]
    [Fact]
    public void Remove_WhenObjectDoesNotExist_ShouldThrow()
    {
        throw new NotImplementedException();
    }
}

