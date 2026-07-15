using System;
using System.IO;
using Xunit;
using FluentAssertions;
using DBMS.StorageEngine.FileManagement.Exceptions;
using DBMS.StorageEngine.IntegrationTests.FileManagement.TestSupport;
using DBMS.StorageEngine.FileManagement.RuntimeFileManagement;

namespace DBMS.StorageEngine.IntegrationTests.FileManagement.FileValidation;

public class FileCorruptionDetectionTests
{
    private readonly FileManagementTestCompositionRoot _compositionRoot = new();

    [Trait("TestCaseId", "IT-FM-VAL-001")]
    [Fact]
    public void OpenFile_WhenMagicNumberIsCorrupted_ShouldReportApprovedValidationFailure()
    {
        // Arrange
        using var tempDir = new TemporaryDirectory();
        var filePath = Path.Combine(tempDir.Path, "test_corrupt.db");
        var lifecycleManager = _compositionRoot.FileLifecycleManager;
        var physicalFs = _compositionRoot.PhysicalFileSystem;

        // Ensure a valid physical file exists first.
        var initHandle = physicalFs.Create(filePath, 4096);
        physicalFs.Close(initHandle);

        // Targeted corruption
        using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Write))
        {
            var corruptionPayload = new byte[] { 0xFF, 0xFF, 0xFF, 0xFF };
            fs.Seek(0, SeekOrigin.Begin);
            fs.Write(corruptionPayload, 0, corruptionPayload.Length);
        }

        // Act
        Action act = () => lifecycleManager.OpenFile(filePath, FileAccessMode.ReadWrite, FileLockMode.Exclusive);

        // Assert
        act.Should().Throw<InvalidFileFormatException>();
    }
}
