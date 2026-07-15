using System;
using DBMS.StorageEngine.FileManagement.Domain;
using DBMS.StorageEngine.FileManagement.PhysicalStorage;
using DBMS.StorageEngine.FileManagement.RuntimeFileManagement;

namespace DBMS.StorageEngine.FileManagement.FileIO;

public class FileWriter : IFileWriter
{
    public void WriteAtOffset(OpenFileEntry entry, long offset, ReadOnlyMemory<byte> source)
    {
        throw new NotImplementedException();
    }

    public void WritePage(OpenFileEntry entry, PageId pageId, ReadOnlyMemory<byte> source)
    {
        throw new NotImplementedException();
    }

    public void WriteHeader(FileHandle handle, FileHeader header)
    {
        throw new NotImplementedException();
    }

    public void WriteAllocationMetadata(FileHandle handle, FileHeader header, AllocationMetadata metadata)
    {
        throw new NotImplementedException();
    }

    public void WriteExtentBitmap(FileHandle handle, FileHeader header, AllocationMetadata metadata)
    {
        throw new NotImplementedException();
    }
}
