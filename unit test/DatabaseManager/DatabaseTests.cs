using DBMS.Exceptions;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

public class DatabaseTests
{
    private Database _database;

    public DatabaseTests()
    {
        _database = new Database();
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
        parentTable.AddColumn(new Column("Id", typeof(int)));
        parentSchema.AddTable(parentTable);

        var childSchema = new Schema("ChildSchema");
        var childTable = new Table("ChildTable");
        childTable.AddColumn(new Column("ParentId", typeof(int)));

        var foreignKey = new ForeignKeyConstraint("FK_Child_Parent", "ParentId", parentTable.Name, "Id");
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
}

