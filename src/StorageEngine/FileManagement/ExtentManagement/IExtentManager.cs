namespace DBMS.StorageEngine.FileManagement.ExtentManagement;

public class AllocatedExtent
{
    public virtual int ExtentId { get; set; }
}

public interface IExtentUsageTracker
{
    bool IsInUse(int extentId);
}

public interface IExtentManager
{
    AllocatedExtent AllocateExtent(RuntimeFileManagement.OpenFileEntry entry);
    void FreeExtent(RuntimeFileManagement.OpenFileEntry entry, int extentId);
}
