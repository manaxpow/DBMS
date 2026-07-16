using DBMS.StorageEngine.FileManagement.Domain;
using DBMS.StorageEngine.FileManagement.RuntimeFileManagement;

namespace DBMS.StorageEngine.FileManagement.FileLifecycle;

public interface IFileLifecycleManager
{
    DataFile CreateFile(FilePath fileName, FileType fileType, int pageSize, long initialFileSize);
    OpenFileEntry OpenFile(FilePath fileName, FileAccessMode accessMode, FileLockMode lockMode);
    void CloseFile(FilePath fileName);
    void DeleteFile(FilePath fileName);
    void ResizeFile(OpenFileEntry entry, long newSize);
}
