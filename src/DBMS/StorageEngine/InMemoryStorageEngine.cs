using System.Collections.Generic;

public class InMemoryStorageEngine : IStorageEngine
{
    private readonly Dictionary<PageId, Page> memoryPages = new Dictionary<PageId, Page>();

    public void Mount()
    {
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
