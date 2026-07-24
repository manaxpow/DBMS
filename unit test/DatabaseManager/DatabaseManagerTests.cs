using DBMS.Exceptions;
using FluentAssertions;

public class DatabaseManagerTests
{
    private readonly DatabaseManager _databaseManager;

    public DatabaseManagerTests()
    {
        _databaseManager = new DatabaseManager();
    }
    [Trait("Category", "Important")]
    [Fact]
    public void CreateDatabase_WhenNameIsValid_ShouldRegisterDatabase()
    {
        // Arrange
        var databaseName = "TestDatabase";

        // Act
        _databaseManager.CreateDatabase(databaseName);

        // Assert
        var database = _databaseManager.GetDatabase(databaseName);
        database.Should().NotBeNull();
    }

    [Trait("Category", "Important")]
    [Fact]
    public void CreateDatabase_WhenNameAlreadyExists_ShouldThrow()
    {
        // Arrange
        var databaseName = "TestDatabase";
        _databaseManager.CreateDatabase(databaseName);

        // Act
        Action act = () => _databaseManager.CreateDatabase(databaseName);

        // Assert
        act.Should().Throw<DatabaseAlreadyExistsException>();
    }

    [Trait("Category", "Important")]
    [Fact]
    public void CreateDatabase_WhenCreationFails_ShouldNotRegisterDatabase()
    {
        // Arrange
        var databaseName = "TestDatabase";
        // Simulate a failure in the creation process by throwing an exception
        Action createDatabaseAction = () => throw new DatabaseCreationException();

        // Act
        Action act = () =>
        {
            createDatabaseAction();
            _databaseManager.CreateDatabase(databaseName);
        };
        Action getDatabaseAction = () => _databaseManager.GetDatabase(databaseName);

        // Assert
        act.Should().Throw<DatabaseCreationException>();
        getDatabaseAction.Should().Throw<DatabaseNotFoundException>();
    }


    [Trait("Category", "Important")]
    [Fact]
    public void GetDatabase_WhenDatabaseExists_ShouldReturnDatabase()
    {
        // Arrange
        var databaseName = "TestDatabase";
        _databaseManager.CreateDatabase(databaseName);

        // Act
        var database = _databaseManager.GetDatabase(databaseName);

        // Assert
        database.Should().NotBeNull();
    }

    [Trait("Category", "Important")]
    [Fact]
    public void DropDatabase_WhenDatabaseExists_ShouldRemoveDatabase()
    {
        // Arrange
        var databaseName = "TestDatabase";
        _databaseManager.CreateDatabase(databaseName);

        // Act
        _databaseManager.DropDatabase(databaseName);

        // Assert
        Action act = () => _databaseManager.GetDatabase(databaseName);
        act.Should().Throw<DatabaseNotFoundException>();
    }

    [Trait("Category", "Important")]
    [Fact]
    public void DropDatabase_WhenDatabaseDoesNotExist_ShouldThrow()
    {
        // Arrange
        var databaseName = "NonExistentDatabase";

        // Act
        Action act = () => _databaseManager.DropDatabase(databaseName);

        // Assert
        act.Should().Throw<DatabaseNotFoundException>();
    }

    [Fact]
    public void CreateDatabase_WhenNameIsInvalid_ShouldThrow()
    {
        throw new NotImplementedException();
    }

    [Fact]
    public void GetDatabase_WhenDatabaseDoesNotExist_ShouldReturnNull()
    {
        throw new NotImplementedException();
    }

    [Trait("Category", "Important")]
    [Fact]
    public void Instance_ShouldReturnSameInstance()
    {
        // Act
        var instance1 = DatabaseManager.Instance;
        var instance2 = DatabaseManager.Instance;

        // Assert
        instance1.Should().NotBeNull();
        instance1.Should().BeSameAs(instance2);
    }
}
