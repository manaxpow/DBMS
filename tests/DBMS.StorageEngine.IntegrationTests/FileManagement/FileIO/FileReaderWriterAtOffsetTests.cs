using System;
using System.IO;
using Xunit;
using FluentAssertions;
using DBMS.StorageEngine.FileManagement.RuntimeFileManagement;
using DBMS.StorageEngine.IntegrationTests.FileManagement.TestSupport;

namespace DBMS.StorageEngine.IntegrationTests.FileManagement.FileIO;

public class FileReaderWriterAtOffsetTests
{
    private readonly FileManagementTestCompositionRoot _compositionRoot = new();

    [Trait("TestCaseId", "IT-FM-IO-RW-001")]
    [Fact]
    public void ReadWriteAtOffset_WhenRangeIsValid_ShouldRoundTripBytes()
    {
        // Arrange
        using var tempDir = new TemporaryDirectory();
        var filePath = Path.Combine(tempDir.Path, "test_io_rw.db");
        var lifecycleManager = _compositionRoot.FileLifecycleManager;
        var physicalFs = _compositionRoot.PhysicalFileSystem;
        var fileWriter = _compositionRoot.FileWriter;
        var fileReader = _compositionRoot.FileReader;

        var initHandle = physicalFs.Create(filePath, 4096);
        physicalFs.Close(initHandle);

        OpenFileEntry openEntry = null;
        try
        {
            openEntry = lifecycleManager.OpenFile(filePath, FileAccessMode.ReadWrite, FileLockMode.Exclusive);

            var expectedBytes = new byte[] { 0x1, 0x2, 0x3, 0x4 };
            var actualBytes = new byte[4];

            // Act
            fileWriter.WriteAtOffset(openEntry, 100, expectedBytes);
            
            // To ensure it hits the disk, we close and reopen
            lifecycleManager.CloseFile(filePath);
            openEntry = lifecycleManager.OpenFile(filePath, FileAccessMode.ReadWrite, FileLockMode.Exclusive);

            fileReader.ReadAtOffset(openEntry, 100, actualBytes);

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
