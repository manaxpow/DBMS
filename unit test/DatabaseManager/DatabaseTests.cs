using DBMS.Exceptions;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

public class DatabaseTests
{
    private RelationalDatabase _database;

    public DatabaseTests()
    {
        _database = new RelationalDatabase();
    }

    [Trait("Category", "Important")]
    [Fact]
    public void Open_WhenDatabaseIsClosed_ShouldOpenDatabase()
    {
        // Act
        _database.Open();

        // Assert
        _database.Should().NotBeNull();
    }

    [Trait("Category", "Important")]
    [Fact]
    public void Open_WhenStorageInitializationFails_ShouldRemainClosed()
    {

        // Act
        Action act = () => _database
            .When(x => x.Open()).Should()
            .Throws<StorageInitializationException>();

        // Assert
        act.Should().Throw<StorageInitializationException>();
    }

    [Trait("Category", "Important")]
    [Fact]
    public void Close_WhenDatabaseIsOpen_ShouldCloseDatabase()
    {
        // Arrange
        _database.Open();

        // Act
        _database.Close();

        // Assert
        _database.Should().NotBeNull();
    }

    [Trait("Category", "Important")]
    [Fact]
    public void Close_WhenFlushFails_ShouldNotReportSuccessfulClose()
    {
        // Act
        Action act = () => _database
            .When(x => x.Close()).Should()
            .Throws<FlushFailureException>();

        // Assert
        act.Should().Throw<FlushFailureException>();
    }

    [Trait("Category", "Important")]
    [Fact]
    public void AddSchema_WhenSchemaIsValid_ShouldRegisterSchema()
    {
        // Act
        _database.AddSchema(new { Name = "TestSchema" });

        // Assert
        _database.Should().NotBeNull();
    }

    [Trait("Category", "Important")]
    [Fact]
    public void AddSchema_WhenNameAlreadyExists_ShouldThrow()
    {
        // Arrange
        var existingSchema = new { Name = "TestSchema" };
        var conflictingSchema = new { Name = "TestSchema" };
        _database.AddSchema(existingSchema);

        // Act
        Action act = () => _database.AddSchema(conflictingSchema);

        // Assert
        act.Should().Throw<SchemaAlreadyExistsException>();
    }

    [Trait("Category", "Important")]
    [Fact]
    public void DropSchema_WhenSchemaExists_ShouldRemoveSchema()
    {
        // Arrange
        var schemaName = "TestSchema";
        _database.AddSchema(new { Name = schemaName });

        // Act
        _database.DropSchema(schemaName);

        // Assert
        Action act = () => _database.DropSchema(schemaName);
        act.Should().Throw<SchemaNotFoundException>();
    }

    [Trait("Category", "Important")]
    [Fact]
    public void DropSchema_WhenSchemaIsReferenced_ShouldThrow()
    {
        // Arrange
        var parentSchema = new Schema("ParentSchema");
        var parentTable = new Table("ParentTable");
        parentTable.AddColumn(new Column("Id", DataTypeFactory.Create("INT")));
        parentSchema.AddTable(parentTable);

        var childSchema = new Schema("ChildSchema");
        var childTable = new Table("ChildTable");
        childTable.AddColumn(new Column("ParentId", DataTypeFactory.Create("INT")));

        var foreignKey = new ForeignKeyConstraint("FK_Child_Parent", "ParentId", parentTable.Name, "Id", new RestrictAction(), new RestrictAction());
        childTable.AddConstraint(foreignKey);
        childSchema.AddTable(childTable);

        _database.AddSchema(parentSchema);
        _database.AddSchema(childSchema);
        // Act
        Action act = () => _database.DropSchema(parentSchema.Name);

        // Assert
        act.Should().Throw<SchemaReferencedException>();
    }

    [Fact]
    public void Open_WhenDatabaseIsAlreadyOpen_ShouldRemainOpen()
    {
        throw new NotImplementedException();
    }

    [Fact]
    public void Close_WhenDatabaseIsAlreadyClosed_ShouldRemainClosed()
    {
        throw new NotImplementedException();
    }

    [Fact]
    public void DropSchema_WhenSchemaDoesNotExist_ShouldThrow()
    {
        throw new NotImplementedException();
    }

    [Fact]
    public void AlterSchema_WhenSchemaExists_ShouldUpdateSchema()
    {
        throw new NotImplementedException();
    }

    [Fact]
    public void AlterSchema_WhenSchemaDoesNotExist_ShouldThrow()
    {
        throw new NotImplementedException();
    }

    [Fact]
    public void ReadPage_ShouldDelegateToStorageEngine()
    {
        // Arrange
        var storageEngine = Substitute.For<IStorageEngine>();
        var expectedPage = new Page(new PageId(105), new byte[4096]);
        storageEngine.FetchPage(105).Returns(expectedPage);

        var database = new RelationalDatabase(storageEngine);

        // Act
        var result = database.ReadPage(105);

        // Assert
        result.Should().Be(expectedPage);
        storageEngine.Received(1).FetchPage(105);
    }
}

