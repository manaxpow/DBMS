using DBMS.StorageEngine.FileManagement.RuntimeFileManagement;

namespace DBMS.StorageEngine.FileManagement.RuntimeFileManagement;

public interface IOpenFileManager
{
    OpenFileEntry? GetOpenFile(string fileName);
    void RegisterOpenFile(string fileName, OpenFileEntry entry);
    void UnregisterOpenFile(string fileName);
    bool TryBeginDelete(string fileName);
    void CompleteDelete(string fileName);
    void CancelDelete(string fileName);
}
