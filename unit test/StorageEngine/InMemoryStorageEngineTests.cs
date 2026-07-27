using FluentAssertions;

public class InMemoryStorageEngineTests
{
    [Fact]
    public void FetchPage_ShouldReadFromMemory()
    {
        // Arrange
        var engine = new InMemoryStorageEngine();
        var expectedPage = new Page(new PageId(105), new byte[4096]);
        expectedPage.Data[0] = 42;
        engine.FlushPage(expectedPage);

        // Act
        var page = engine.FetchPage(105);

        // Assert
        page.Should().NotBeNull();
        page.Data[0].Should().Be(42);
    }

    [Fact]
    public void FlushPage_ShouldWriteToMemory()
    {
        // Arrange
        var engine = new InMemoryStorageEngine();
        var page = new Page(new PageId(105), new byte[4096]);
        page.Data[0] = 99;

        // Act
        engine.FlushPage(page);

        // Assert
        var fetchedPage = engine.FetchPage(105);
        fetchedPage.Data[0].Should().Be(99);
    }
}
