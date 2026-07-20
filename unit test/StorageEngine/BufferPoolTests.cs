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


    [Fact]
    public void FetchPage_WhenPageIsBuffered_ShouldIncrementPinCount()
    {
        throw new NotImplementedException();
    }

    [Fact]
    public void FetchPage_WhenPageIsBuffered_ShouldNotReadFromFile()
    {
        throw new NotImplementedException();
    }

    [Fact]
    public void FetchPage_WhenNoFreeFrameAndCleanVictimExists_ShouldEvictVictim()
    {
        throw new NotImplementedException();
    }

    [Fact]
    public void FetchPage_WhenNoFreeFrameAndDirtyVictimExists_ShouldFlushThenEvictVictim()
    {
        throw new NotImplementedException();
    }

    [Fact]
    public void FetchPage_WhenFileReadFails_ShouldNotRegisterPage()
    {
        throw new NotImplementedException();
    }

    [Fact]
    public void Flush_WhenPageIsDirty_ShouldWriteToDisk()
    {
        throw new NotImplementedException();
    }

    [Fact]
    public void Flush_WhenPageIsClean_ShouldNotWriteToDisk()
    {
        throw new NotImplementedException();
    }

    [Fact]
    public void Flush_WhenPageIsNotBuffered_ShouldThrow()
    {
        throw new NotImplementedException();
    }

    [Fact]
    public void Unpin_WhenPageIsPinned_ShouldDecreasePinCount()
    {
        throw new NotImplementedException();
    }

    [Fact]
    public void Unpin_WhenPinCountIsZero_ShouldThrow()
    {
        throw new NotImplementedException();
    }

    [Fact]
    public void Unpin_WhenMarkedDirty_ShouldSetDirtyFlag()
    {
        throw new NotImplementedException();
    }

    [Fact]
    public void Evict_WhenFrameIsUnpinned_ShouldFreeSpace()
    {
        throw new NotImplementedException();
    }

    [Fact]
    public void Evict_WhenFrameIsPinned_ShouldThrow()
    {
        throw new NotImplementedException();
    }
}
