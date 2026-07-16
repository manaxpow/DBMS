using System;
using DBMS.StorageEngine.FileManagement.Domain;
using DBMS.StorageEngine.FileManagement.PhysicalStorage;
using DBMS.StorageEngine.FileManagement.RuntimeFileManagement;

namespace DBMS.StorageEngine.FileManagement.FileIO;

public class FileReader : IFileReader
{
    public int ReadAtOffset(OpenFileEntry entry, long offset, Memory<byte> destination)
    {
        throw new NotImplementedException();
    }

    public void ReadPage(OpenFileEntry entry, PageId pageId, Memory<byte> destination)
    {
        throw new NotImplementedException();
    }

    public FileHeader ReadHeader(FileHandle handle)
    {
        throw new NotImplementedException();
    }

    public AllocationMetadata ReadAllocationMetadata(FileHandle handle, FileHeader header)
    {
        throw new NotImplementedException();
    }
}
