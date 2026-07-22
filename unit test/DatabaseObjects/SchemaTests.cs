using FluentAssertions;
using DBMS.Exceptions;

public class SchemaTests
{
    private Schema _schema;
    public SchemaTests()
    {
        _schema = new Schema("TestSchema");
    }

    [Trait("Category", "Important")]
    [Fact]
    public void AddTable_WhenTableIsValid_ShouldRegisterTable()
    {
        // Arrange
        var table = new Table("TestTable");

        // Act
        var isContainedBeforeAdd = _schema.ContainsTable("TestTable");
        _schema.AddTable(table);

        // Assert
        isContainedBeforeAdd.Should().Be(false);
        _schema.GetTable("TestTable").Should().BeSameAs(table);
    }

    [Fact]
    public void AddTable_WhenTableIsNull_ShouldThrow()
    {
        // Act
        Action action = () => _schema.AddTable(null);
        // Assert
        action.Should().Throw<ArgumentNullException>();
    }

    [Trait("Category", "Important")]
    [Fact]
    public void AddTable_WhenNameAlreadyExists_ShouldThrow()
    {
        // Arrange
        var existingTable = new Table("TestTable");
        var newTable = new Table("TestTable");
        _schema.AddTable(existingTable);

        // Act
        Action action = () => _schema.AddTable(newTable);
        // Assert
        _schema.ContainsTable("TestTable").Should().Be(true);
        action.Should().Throw<TableAlreadyExistsException>();
    }

    [Fact]
    public void GetTable_WhenTableExists_ShouldReturnTable()
    {
        // Arrange
        var table = new Table("TestTable");
        _schema.AddTable(table);

        // Act
        var retrievedTable = _schema.GetTable("TestTable");

        // Assert
        retrievedTable.Should().BeSameAs(table);
    }

    [Fact]
    public void GetTable_WhenTableDoesNotExist_ShouldReturnNull()
    {
        // Act
        var retrievedTable = _schema.GetTable("NonExistentTable");

        // Assert
        retrievedTable.Should().BeNull();
    }

    [Fact]
    public void ContainsTable_WhenTableExists_ShouldReturnTrue()
    {
        // Arrange
        var table = new Table("TestTable");
        _schema.AddTable(table);

        // Act
        var containsTable = _schema.ContainsTable("TestTable");

        // Assert
        containsTable.Should().Be(true);
    }

    [Fact]
    public void ContainsTable_WhenTableDoesNotExist_ShouldReturnFalse()
    {
        // Act
        var containsTable = _schema.ContainsTable("NonExistentTable");

        // Assert
        containsTable.Should().Be(false);
    }

    [Trait("Category", "Important")]
    [Fact]
    public void DropTable_WhenTableIsNotReferenced_ShouldRemoveTable()
    {
        // Arrange
        var table = new Table("TestTable");
        _schema.AddTable(table);

        // Act
        var isContainedBeforeDrop = _schema.ContainsTable("TestTable");
        _schema.DropTable("TestTable");

        // Assert
        isContainedBeforeDrop.Should().Be(true);
        _schema.ContainsTable("TestTable").Should().Be(false);
    }

    [Trait("Category", "Important")]
    [Fact]
    public void DropTable_WhenTableIsReferencedByForeignKey_ShouldThrow()
    {
        // Arrange
        var childTable = new Table("TestTable");
        childTable.AddColumn(new Column("Id", typeof(int)));
        childTable.AddColumn(new Column("ForeignKeyId", typeof(int)));

        var referencedTable = new Table("TableReferenced");
        referencedTable.AddColumn(new Column("Id", typeof(int)));
        _schema.AddTable(referencedTable);
        _schema.AddTable(childTable);

        var foreignKey = new ForeignKeyConstraint("FK_TestTable_TableReferenced", "ForeignKeyId", "TableReferenced", "Id", new RestrictAction(), new RestrictAction());
        childTable.AddConstraint(foreignKey);

        // Act & Assert
        Action action = () => _schema.DropTable("TableReferenced");
        action.Should().Throw<TableReferencedException>();
    }

    [Trait("Category", "Important")]
    [Fact]
    public void DropTable_WhenTableDoesNotExist_ShouldThrow()
    {
        // Arrange
        Action action = () => _schema.DropTable("NonExistentTable");

        // Assert
        action.Should().Throw<TableNotFoundException>();
    }

    [Fact]
    public void AlterTable_WhenTableExists_ShouldUpdateTable()
    {
        // Arrange
        var originalTable = new Table("TestTable");
        _schema.AddTable(originalTable);

        var newTable = new Table("TestTable");
        newTable.AddColumn(new Column("NewColumn", typeof(int)));

        // Act
        _schema.AlterTable("TestTable", newTable);

        // Assert
        var alteredTable = _schema.GetTable("TestTable");
        alteredTable.Should().BeSameAs(newTable);
        alteredTable.Columns.Should().ContainSingle(c => c.Name == "NewColumn");
    }

    [Fact]
    public void AlterTable_WhenTableDoesNotExist_ShouldThrow()
    {
        // Arrange
        var newTable = new Table("NonExistentTable");

        // Act
        Action action = () => _schema.AlterTable("NonExistentTable", newTable);

        // Assert
        action.Should().Throw<TableNotFoundException>();
    }
    [Trait("Category", "Important")]
    [Fact]
    public void Drop_WhenSchemaIsEmpty_ShouldNotThrow()
    {
        // Act
        Action action = () => _schema.Drop();

        // Assert
        action.Should().NotThrow();
    }

    [Fact]
    public void DropSchema_WhenSchemaContainsObjects_ShouldThrow()
    {
        // Arrange
        var schema = new Schema("TestSchema");
        schema.RegisterObject(new Table("TestTable"));

        var schemaManager = new SchemaManager();

        // Act
        Action action = () =>
            schemaManager.DropSchema(schema, cascade: false);

        // Assert
        action.Should()
            .Throw<SchemaNotEmptyException>();
    }

    [Trait("Category", "Important")]
    [Fact]
    public void DropSchema_WhenCascadeIsTrue_ShouldDropAllSchemaObjectsAndNotThrow()
    {
        // Arrange
        var schema = new Schema("TestSchema");
        schema.RegisterObject(new Table("TestTable"));

        var schemaManager = new SchemaManager();

        // Act
        schemaManager.DropSchema(schema, cascade: true);

        // Assert
        schema.Objects.Should().BeEmpty();
    }
}
