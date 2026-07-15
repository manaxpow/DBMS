using System;

namespace DBMS.StorageEngine.FileManagement.Domain;

public class AllocationMetadata
{
    public int ExtentSize { get; private set; }
    public int TotalExtentCount { get; private set; }
    public int FreeExtentCount { get; private set; }
    public ExtentBitmap ExtentBitmap { get; private set; }

    internal AllocationMetadata() 
    {
    }

    private AllocationMetadata(int extentSize, int initialExtents)
    {
        ExtentSize = extentSize;
        TotalExtentCount = initialExtents;
    }

    public static AllocationMetadata Create(int extentSize, int initialExtents)
    {
        return new AllocationMetadata(extentSize, initialExtents);
    }

    public virtual int? FindFreeExtent()
    {
        throw new NotImplementedException();
    }
    
    public virtual bool ContainsExtent(int extentId)
    {
        throw new NotImplementedException();
    }
    
    public virtual string GetExtentState(int extentId)
    {
        throw new NotImplementedException();
    }

    public virtual void MarkExtentAllocated(int extentId)
    {
        throw new NotImplementedException();
    }

    public virtual void MarkExtentFree(int extentId)
    {
        throw new NotImplementedException();
    }

    public virtual void AddExtents(int count)
    {
        throw new NotImplementedException();
    }

    public virtual bool CanTruncateTo(int extentCount)
    {
        throw new NotImplementedException();
    }

    public virtual void TruncateTo(int extentCount)
    {
        throw new NotImplementedException();
    }

    public virtual long CalculateFileOffset(int extentId, FileHeader header)
    {
        throw new NotImplementedException();
    }
}
