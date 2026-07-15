using System;
using System.Collections;

namespace DBMS.StorageEngine.FileManagement.Domain;

public class ExtentBitmap
{
    public int TotalExtents { get; private set; }

    internal ExtentBitmap()
    {
        throw new NotImplementedException();
    }

    private ExtentBitmap(int totalExtents)
    {
        throw new NotImplementedException();
    }

    public static ExtentBitmap Create(int totalExtents)
    {
        throw new NotImplementedException();
    }

    public virtual int? FindFirstFree()
    {
        throw new NotImplementedException();
    }

    public virtual bool IsAllocated(int index)
    {
        throw new NotImplementedException();
    }

    public virtual void MarkUsed(int index)
    {
        throw new NotImplementedException();
    }

    public virtual void MarkFree(int index)
    {
        throw new NotImplementedException();
    }

    public virtual void AppendFreeExtents(int count)
    {
        throw new NotImplementedException();
    }

    public virtual void Truncate(int newTotalExtents)
    {
        throw new NotImplementedException();
    }
}
