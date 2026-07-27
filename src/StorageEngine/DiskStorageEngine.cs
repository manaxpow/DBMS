using System.IO;

public class DiskStorageEngine : IStorageEngine
{
    private readonly string _storageDir = "disk_storage";

    public void Mount() 
    { 
        if (!Directory.Exists(_storageDir))
        {
            Directory.CreateDirectory(_storageDir);
        }
    }

    public Page FetchPage(int pageId) 
    { 
        var path = Path.Combine(_storageDir, $"{pageId}.page");
        if (File.Exists(path))
        {
            var data = File.ReadAllBytes(path);
            return new Page(new PageId(pageId), data);
        }
        return new Page(new PageId(pageId), new byte[4096]); 
    }

    public void FlushPage(Page page) 
    { 
        Mount(); // ensure directory exists
        var path = Path.Combine(_storageDir, $"{page.PageId.Value}.page");
        File.WriteAllBytes(path, page.Data);
    }
}
