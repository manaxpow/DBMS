using System.IO;

public class DiskStorageEngine : IStorageEngine
{
    private readonly string _storageDir = "disk_storage";

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
