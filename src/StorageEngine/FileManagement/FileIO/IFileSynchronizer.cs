using DBMS.StorageEngine.FileManagement.RuntimeFileManagement;

namespace DBMS.StorageEngine.FileManagement.FileIO;

public interface IFileSynchronizer
{
    void Sync(OpenFileEntry entry);
}
