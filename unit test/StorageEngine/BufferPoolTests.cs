using System;
using NSubstitute;
using Xunit;

public class BufferPoolTests
{
    private readonly IFileManager _fileManager;
    private readonly BufferPool _bufferPool;
    public BufferPoolTests()
    {
        _fileManager = Substitute.For<IFileManager>();
        _bufferPool = new BufferPool(10, _fileManager);
    }

    [Fact]
    public void FetchPage_WhenPageIsBuffered_ShouldReturnExistingFrame()
    {
        // Arrange
        var pageId = new PageId(1);
        var frameId = new FrameId(1);

        var page = new Page(pageId, new byte[4096]);
        var frame = new Frame(frameId, page);

        // Add frame to buffer pool
        _bufferPool.PageTable[pageId] = frame;

        // Act
        var result = _bufferPool.FetchPage(pageId);

        // Assert
        Assert.Same(frame, result);

        _fileManager.DidNotReceive().ReadPage(Arg.Any<PageId>());
    }

    [Fact]
    public void FetchPage_WhenSpaceIsAvailable_ShouldLoadPage()
    {
        throw new NotImplementedException();
    }

    [Fact]
    public void FetchPage_WhenAllFramesArePinned_ShouldThrow()
    {
        throw new NotImplementedException();
    }

}
