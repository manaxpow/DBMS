using System;

namespace DBMS.StorageEngine.FileManagement.RuntimeFileManagement;

public class OpenFileManager : IOpenFileManager
{
    public OpenFileEntry? GetOpenFile(string fileName)
    {
        throw new NotImplementedException();
    }
    
    public void RegisterOpenFile(string fileName, OpenFileEntry entry)
    {
        throw new NotImplementedException();
    }
    
    public void UnregisterOpenFile(string fileName)
    {
        throw new NotImplementedException();
    }
    
    public bool TryBeginDelete(string fileName)
    {
        throw new NotImplementedException();
    }
    
    public void CompleteDelete(string fileName)
    {
        throw new NotImplementedException();
    }
    
    public void CancelDelete(string fileName)
    {
        throw new NotImplementedException();
    }
}
