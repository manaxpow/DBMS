using DBMS.StorageEngine.FileManagement.Domain;
using DBMS.StorageEngine.FileManagement.RuntimeFileManagement;
using DBMS.StorageEngine.FileManagement.PhysicalStorage;
using DBMS.StorageEngine.FileManagement.FileIO;

namespace DBMS.StorageEngine.FileManagement.FileLifecycle;

public class FileLifecycleManager : IFileLifecycleManager
{
    private readonly IPhysicalFileSystem _physicalFileSystem;
    private readonly IFileReader _fileReader;
    private readonly IFileWriter _fileWriter;
    private readonly IFileSynchronizer _fileSynchronizer;
    private readonly IOpenFileManager _openFileManager;
    private readonly IFileValidator _fileValidator;

    public FileLifecycleManager(
        IPhysicalFileSystem physicalFileSystem,
        IFileReader fileReader,
        IFileWriter fileWriter,
        IFileSynchronizer fileSynchronizer,
        IOpenFileManager openFileManager,
        IFileValidator fileValidator)
    {
        _physicalFileSystem = physicalFileSystem;
        _fileReader = fileReader;
        _fileWriter = fileWriter;
        _fileSynchronizer = fileSynchronizer;
        _openFileManager = openFileManager;
        _fileValidator = fileValidator;
    }

    public DataFile CreateFile(FilePath fileName, FileType fileType, int pageSize, long initialFileSize)
    {
        throw new System.NotImplementedException();
    }

    public OpenFileEntry OpenFile(FilePath fileName, FileAccessMode accessMode, FileLockMode lockMode)
    {
        throw new System.NotImplementedException();
    }

    public void CloseFile(FilePath fileName)
    {
        throw new System.NotImplementedException();
    }

    public void DeleteFile(FilePath fileName)
    {
        throw new System.NotImplementedException();
    }

    public void ResizeFile(OpenFileEntry entry, long newSize)
    {
        throw new System.NotImplementedException();
    }
}
