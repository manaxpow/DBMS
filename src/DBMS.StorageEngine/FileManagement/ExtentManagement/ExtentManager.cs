using System;

namespace DBMS.StorageEngine.FileManagement.ExtentManagement;

public class ExtentManager : IExtentManager
{
    public AllocatedExtent AllocateExtent(RuntimeFileManagement.OpenFileEntry entry)
    {
        throw new NotImplementedException();
    }

    public void FreeExtent(RuntimeFileManagement.OpenFileEntry entry, int extentId)
    {
        throw new NotImplementedException();
    }
}
