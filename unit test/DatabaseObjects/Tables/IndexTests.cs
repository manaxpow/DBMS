using FluentAssertions;

public class IndexTests
{
    private readonly Index _index;

    public IndexTests()
    {
        _index = new Index("IX_ParentId", new List<string> { "ParentId" }, isUnique: true);
    }
    [Trait("Category", "Important")]
    [Fact]
    public void Insert_WhenKeyIsValid_ShouldAddEntry()
    {
        // Arrange
        var recordPointer = new object();

        // Act
        _index.Insert("TestKey", recordPointer);
        var result = _index.Search("TestKey");

        // Assert
        result.Should().Be(recordPointer);
    }

    [Trait("Category", "Important")]
    [Fact]
    public void Insert_WhenUniqueKeyAlreadyExists_ShouldThrow()
    {
        // Arrange
        var recordPointer = new object();
        _index.Insert("TestKey", recordPointer);

        // Act
        Action act = () => _index.Insert("TestKey", new object());

        // Assert
        act.Should().Throw<InvalidOperationException>();
    }

    [Trait("Category", "Important")]
    [Fact]
    public void Insert_WhenIndexIsNonUnique_ShouldAllowDuplicateKeys()
    {
        // Arrange
        var nonUniqueIndex = new Index("IX_ParentId", new List<string> { "ParentId" }, isUnique: false);
        var recordPointer1 = new object();
        var recordPointer2 = new object();

        // Act
        nonUniqueIndex.Insert("TestKey", recordPointer1);
        nonUniqueIndex.Insert("TestKey", recordPointer2);
        var result = nonUniqueIndex.Search("TestKey");

        // Assert
        result.Should().Be(recordPointer1);
    }

    [Trait("Category", "Important")]
    [Fact]
    public void Search_WhenKeyExists_ShouldReturnRecordPointer()
    {
        // Arrange
        var recordPointer = new object();
        _index.Insert("TestKey", recordPointer);

        // Act
        var result = _index.Search("TestKey");

        // Assert
        result.Should().Be(recordPointer);
    }

    [Trait("Category", "Important")]
    [Fact]
    public void Search_WhenKeyDoesNotExist_ShouldReturnNull()
    {
        // Act
        var result = _index.Search("NonExistentKey");

        // Assert
        result.Should().BeNull();
    }

    [Trait("Category", "Important")]
    [Fact]
    public void Delete_WhenKeyExists_ShouldRemoveEntry()
    {
        // Arrange
        var recordPointer = new object();
        _index.Insert("TestKey", recordPointer);

        // Act
        var result = _index.Delete("TestKey");

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void Delete_WhenKeyDoesNotExist_ShouldReturnFalse()
    {
        throw new NotImplementedException();
    }

    [Trait("Category", "Important")]
    [Fact]
    public void Update_WhenKeyExists_ShouldReplaceRecordPointer()
    {
        // Arrange
        var originalRecordPointer = new object();
        var newRecordPointer = new object();
        _index.Insert("TestKey", originalRecordPointer);

        // Act
        _index.Update("TestKey", newRecordPointer);
        var result = _index.Search("TestKey");

        // Assert
        result.Should().Be(newRecordPointer);
    }

    [Trait("Category", "Important")]
    [Fact]
    public void Insert_WhenKeyIsNullAndNullsAreNotAllowed_ShouldThrow()
    {
        // Arrange
        var index = new Index("IX_ParentId", new List<string> { "ParentId" }, isUnique: true, allowsNull: false);

        // Act
        Action act = () => index.Insert(null, new object());

        // Assert
        act.Should().Throw<InvalidIndexKeyException>();
    }

    [Fact]
    public void RangeSearch_WhenKeysMatch_ShouldReturnOrderedEntries()
    {
        throw new NotImplementedException();
    }
}


