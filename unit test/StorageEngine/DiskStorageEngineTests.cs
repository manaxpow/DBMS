using System.IO;
using FluentAssertions;

public class DiskStorageEngineTests : IDisposable
{
    public DiskStorageEngineTests()
    {
        if (Directory.Exists("disk_storage"))
        {
            Directory.Delete("disk_storage", true);
        }
    }

    public void Dispose()
    {
        if (Directory.Exists("disk_storage"))
        {
            Directory.Delete("disk_storage", true);
        }
    }

    [Fact]
    public void FetchPage_ShouldReadFromDisk()
    {
        // Arrange
        var engine = new DiskStorageEngine();
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
    public void FlushPage_ShouldWriteToDisk()
    {
        // Arrange
        var engine = new DiskStorageEngine();
        var page = new Page(new PageId(105), new byte[4096]);
        page.Data[0] = 99;

        // Act
        engine.FlushPage(page);

        // Assert
        var fetchedPage = engine.FetchPage(105);
        fetchedPage.Data[0].Should().Be(99);
    }
}
