using System;

public class BufferPoolManager : IBufferPoolManager
{
    private BufferPool _pool;
    private IPhysicalFileSystem _fileSystem;
    private IPageReplacementPolicy _replacer;

    public BufferPoolManager(IPhysicalFileSystem fileSystem, IPageReplacementPolicy replacer)
    {
        _fileSystem = fileSystem;
        _replacer = replacer;
    }

    public FrameId FindFreeFrame()
    {
        return default;
    }

    public Page FetchPage(PageId pageId)
    {
        return default;
    }

    public void UnpinPage(PageId pageId, bool isDirty)
    {
    }

    public void FlushPage(PageId pageId)
    {
    }

    public Page NewPage(FileId fileId)
    {
        return default;
    }

    public void DeletePage(PageId pageId)
    {
    }
}
