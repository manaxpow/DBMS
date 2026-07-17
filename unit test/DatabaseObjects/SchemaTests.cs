public class SchemaTests
{
    private Schema _schema;
    public SchemaTests()
    {
        _schema = new Schema();
    }
    [Fact]
    public void AddTable_WhenTableIsValid_ShouldRegisterTable()
    {
        // Arrange
        var table = new Table("TestTable");

        // Act
        var isContainedBeforeAdd = _schema.ContainsTable("TestTable");
        _schema.AddTable(table);

        // Assert
        Assert.True(isContainedBeforeAdd == false);
        Assert.Same(table, _schema.GetTable("TestTable"));
    }

    [Fact]
    public void AddTable_WhenNameAlreadyExists_ShouldThrow()
    {
        // Arrange
        var existingTable = new Table("TestTable");
        var newTable = new Table("TestTable");
        _schema.AddTable(existingTable);

        // Act & Assert
        Assert.True(_schema.ContainsTable("TestTable"));
        Assert.Throws<DuplicateTableNameException>(() => _schema.AddTable(newTable));
    }

    [Fact]
    public void RemoveTable_WhenTableExists_ShouldRemoveTable()
    {
        // Arrange
        var table = new Table("TestTable");
        _schema.AddTable(table);

        // Act
        var isContainedBeforeRemove = _schema.ContainsTable("TestTable");
        _schema.RemoveTable("TestTable");

        // Assert
        Assert.True(isContainedBeforeRemove == true);
        Assert.False(_schema.ContainsTable("TestTable"));
    }
}
