using System;
using System.Collections.Generic;



public class BufferPool
{
    public int Capacity { get; set; }
    public Dictionary<int, Frame> PageTable { get; set; }
    private IFileManager _iFIleManager;
    public object FetchPage(object pageId)
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

    private object FindBufferedFrame(object pageId)
    {
        throw new NotImplementedException();
    }

    private object FindAvailableFrame()
    {
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
