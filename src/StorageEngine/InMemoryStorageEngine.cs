using System.Collections.Generic;

public class InMemoryStorageEngine : IStorageEngine
{
    private readonly Dictionary<int, Page> _pages = new();

    public void Mount() { }

    public Page FetchPage(int pageId)
    {
        throw new NotImplementedException();
    }

    public void FlushPage(Page page)
    {
        throw new NotImplementedException();
    }
}
