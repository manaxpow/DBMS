using System;
using DBMS.StorageEngine.FileManagement.PhysicalStorage;
using DBMS.StorageEngine.FileManagement.FileIO;
using DBMS.StorageEngine.FileManagement.FileValidation;
using DBMS.StorageEngine.FileManagement.FileLifecycle;
using DBMS.StorageEngine.FileManagement.ExtentManagement;
using DBMS.StorageEngine.FileManagement.RuntimeFileManagement;

namespace DBMS.StorageEngine.IntegrationTests.FileManagement.TestSupport;

internal sealed class FileManagementTestCompositionRoot
{
    public IPhysicalFileSystem PhysicalFileSystem { get; }
    public IFileReader FileReader { get; }
    public IFileWriter FileWriter { get; }
    public IFileSynchronizer FileSynchronizer { get; }
    public IOpenFileManager OpenFileManager { get; }
    public IFileValidator FileValidator { get; }
    public IFileLifecycleManager FileLifecycleManager { get; }
    public IExtentManager ExtentManager { get; }

    public FileManagementTestCompositionRoot()
    {
        PhysicalFileSystem = new PhysicalFileSystem();
        FileReader = new FileReader();
        FileWriter = new FileWriter();
        FileSynchronizer = new FileSynchronizer();
        OpenFileManager = new OpenFileManager();
        FileValidator = new FileValidator();

        FileLifecycleManager = new FileLifecycleManager(
            PhysicalFileSystem,
            FileReader,
            FileWriter,
            FileSynchronizer,
            OpenFileManager,
            FileValidator);

        ExtentManager = new ExtentManager();
    }
}
