using System;
using System.IO;
using DBMS.StorageEngine.FileManagement.Domain;
using DBMS.StorageEngine.FileManagement.RuntimeFileManagement;

namespace DBMS.StorageEngine.FileManagement.PhysicalStorage;

public class PhysicalFileSystem : IPhysicalFileSystem
{
    public bool Exists(FilePath fileName)
    {
        throw new NotImplementedException();
    }

    public FileHandle Create(FilePath fileName, long initialFileSize)
    {
        throw new NotImplementedException();
    }

    public FileHandle Open(FilePath fileName, FileAccessMode accessMode)
    {
        throw new NotImplementedException();
    }

    public void Close(FileHandle handle)
    {
        throw new NotImplementedException();
    }

    public void Delete(FilePath fileName)
    {
        throw new NotImplementedException();
    }

    public void Resize(FileHandle handle, long newSize)
    {
        throw new NotImplementedException();
    }

    public long GetSize(FileHandle handle)
    {
        throw new NotImplementedException();
    }
}
