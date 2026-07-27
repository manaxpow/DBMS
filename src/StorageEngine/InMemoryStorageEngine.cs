using System.Collections.Generic;

public class InMemoryStorageEngine : IStorageEngine
{
    private readonly Dictionary<int, Page> _pages = new();

    public void Mount() { }

    public Page FetchPage(int pageId) 
    { 
        if (_pages.TryGetValue(pageId, out var page))
        {
            return page;
        }
        return new Page(new PageId(pageId), new byte[4096]); 
    }

    public void FlushPage(Page page) 
    { 
        _pages[page.PageId.Value] = page;
    }
}
