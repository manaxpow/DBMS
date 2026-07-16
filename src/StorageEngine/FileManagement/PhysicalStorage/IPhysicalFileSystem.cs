using DBMS.StorageEngine.FileManagement.Domain;
using DBMS.StorageEngine.FileManagement.RuntimeFileManagement;

namespace DBMS.StorageEngine.FileManagement.PhysicalStorage;

public interface IPhysicalFileSystem
{
    bool Exists(FilePath fileName);
    FileHandle Create(FilePath fileName, long initialFileSize);
    FileHandle Open(FilePath fileName, FileAccessMode accessMode);
    void Close(FileHandle handle);
    void Delete(FilePath fileName);
    void Resize(FileHandle handle, long newSize);
    long GetSize(FileHandle handle);
}
