using System;
using System.Collections.Generic;

public class BufferPool
{
    private readonly IFileManager fileManager;

    public BufferPool(int capacity, IFileManager fileManager)
    {
        this.Capacity = capacity;
        this.fileManager = fileManager;
        this.PageTable = new Dictionary<PageId, Frame>();
    }

    public int Capacity { get; set; }

    public Dictionary<PageId, Frame> PageTable { get; set; }

    public Frame FetchPage(PageId pageId)
    {
        throw new NotImplementedException();
    }

    public void FlushDirtyPages()
    {
        throw new NotImplementedException();
    }

    public void Clear()
    {
        throw new NotImplementedException();
    }

    public void Flush(PageId pageId)
    {
        throw new NotImplementedException();
    }

    public void Evict(object frame)
    {
        throw new NotImplementedException();
    }

    private object FindBufferedFrame(object pageId)
    {
        throw new NotImplementedException();
    }

    private object FindAvailableFrame()
    {
        throw new NotImplementedException();
    }

    private Frame? ExistingFrame(PageId pageId)
    {
        if (this.PageTable.TryGetValue(pageId, out var frame))
        {
            throw new NotImplementedException();
        }

        throw new NotImplementedException();
    }

    private object FindUnpinnedVictim()
    {
        throw new NotImplementedException();
    }

    private void Pin(object frame)
    {
        throw new NotImplementedException();
    }

    private void LoadPage(object frame, object pageData)
    {
        throw new NotImplementedException();
    }

    private void RegisterPage(object pageId, object frame)
    {
        throw new NotImplementedException();
    }
}
