using System;
using System.IO;
using Xunit;
using FluentAssertions;
using DBMS.StorageEngine.IntegrationTests.FileManagement.TestSupport;

namespace DBMS.StorageEngine.IntegrationTests.FileManagement.FileIO;

public class FileSynchronizerPersistenceTests
{
    private readonly FileManagementTestCompositionRoot _compositionRoot = new();

    [Trait("TestCaseId", "IT-FM-IO-SYNC-001")]
    [Fact]
    public void SyncFile_AfterWrite_ShouldEnsureDataRemainsAvailableAfterReopen()
    {
        // Arrange
        using var tempDir = new TemporaryDirectory();
        var filePath = Path.Combine(tempDir.Path, "test_io_sync.db");
        var lifecycleManager = _compositionRoot.FileLifecycleManager;
        var physicalFs = _compositionRoot.PhysicalFileSystem;
        var fileWriter = _compositionRoot.FileWriter;
        var fileReader = _compositionRoot.FileReader;
        var fileSynchronizer = _compositionRoot.FileSynchronizer;

        var initHandle = physicalFs.Create(filePath, 4096);
        physicalFs.Close(initHandle);

        var openEntry = lifecycleManager.OpenFile(filePath, DBMS.StorageEngine.FileManagement.RuntimeFileManagement.FileAccessMode.ReadWrite, DBMS.StorageEngine.FileManagement.RuntimeFileManagement.FileLockMode.Exclusive);

        try
        {
            var expectedBytes = new byte[] { 0x5, 0x6, 0x7, 0x8 };
            var actualBytes = new byte[4];

            // Act
            fileWriter.WriteAtOffset(openEntry, 0, expectedBytes);
            fileSynchronizer.Sync(openEntry);

            lifecycleManager.CloseFile(filePath);
            openEntry = lifecycleManager.OpenFile(filePath, DBMS.StorageEngine.FileManagement.RuntimeFileManagement.FileAccessMode.ReadWrite, DBMS.StorageEngine.FileManagement.RuntimeFileManagement.FileLockMode.Exclusive);

            fileReader.ReadAtOffset(openEntry, 0, actualBytes);

            // Assert
            actualBytes.Should().Equal(expectedBytes);
        }
        finally
        {
            if (openEntry != null)
                lifecycleManager.CloseFile(filePath);
        }
    }
}
