using DBMS.StorageEngine.FileManagement.Domain;
using DBMS.StorageEngine.FileManagement.PhysicalStorage;

namespace DBMS.StorageEngine.FileManagement.RuntimeFileManagement;

public class OpenFileEntry
{
    public virtual DataFile DataFile { get; }
    public virtual FileHandle Handle { get; }
    public virtual FileAccessMode AccessMode { get; }
    public virtual FileLockMode LockMode { get; }
    public virtual int ReferenceCount { get; private set; }
    public virtual bool IsDeleted { get; set; }

    protected OpenFileEntry() { }

    public OpenFileEntry(DataFile dataFile, FileHandle handle, FileAccessMode accessMode, FileLockMode lockMode)
    {
        DataFile = dataFile;
        Handle = handle;
        AccessMode = accessMode;
        LockMode = lockMode;
        ReferenceCount = 1;
    }

    public virtual int DecrementRefCount()
    {
        ReferenceCount--;
        return ReferenceCount;
    }
}
