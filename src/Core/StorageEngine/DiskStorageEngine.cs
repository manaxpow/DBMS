using System.IO;

public class DiskStorageEngine : IStorageEngine
{
    public void Mount()
    {
        throw new NotImplementedException();
    }

    public Page FetchPage(int pageId)
    {
        throw new NotImplementedException();
    }

    public void FlushPage(Page page)
    {
        throw new NotImplementedException();
    }
}
