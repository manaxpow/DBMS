using System;
using DBMS.StorageEngine.FileManagement.Domain;
using DBMS.StorageEngine.FileManagement.RuntimeFileManagement;
using DBMS.StorageEngine.FileManagement.PhysicalStorage;

namespace DBMS.StorageEngine.FileManagement.FileIO;

public interface IFileReader
{
    int ReadAtOffset(OpenFileEntry entry, long offset, Memory<byte> destination);
    FileHeader ReadHeader(FileHandle handle);
    AllocationMetadata ReadAllocationMetadata(FileHandle handle, FileHeader header);
    void ReadPage(OpenFileEntry entry, PageId pageId, Memory<byte> destination);
}
