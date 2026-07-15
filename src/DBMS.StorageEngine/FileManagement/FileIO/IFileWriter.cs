using System;
using DBMS.StorageEngine.FileManagement.Domain;
using DBMS.StorageEngine.FileManagement.RuntimeFileManagement;
using DBMS.StorageEngine.FileManagement.PhysicalStorage;

namespace DBMS.StorageEngine.FileManagement.FileIO;

public interface IFileWriter
{
    void WriteAtOffset(OpenFileEntry entry, long offset, ReadOnlyMemory<byte> source);
    void WriteHeader(FileHandle handle, FileHeader header);
    void WriteAllocationMetadata(FileHandle handle, FileHeader header, AllocationMetadata metadata);
    void WritePage(OpenFileEntry entry, PageId pageId, ReadOnlyMemory<byte> source);
    void WriteExtentBitmap(FileHandle handle, FileHeader header, AllocationMetadata metadata);
}
