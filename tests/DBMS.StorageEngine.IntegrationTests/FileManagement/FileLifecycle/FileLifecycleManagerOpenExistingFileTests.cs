using System;
using System.IO;
using Xunit;
using FluentAssertions;
using DBMS.StorageEngine.FileManagement.RuntimeFileManagement;
using DBMS.StorageEngine.IntegrationTests.FileManagement.TestSupport;

namespace DBMS.StorageEngine.IntegrationTests.FileManagement.FileLifecycle;

public class FileLifecycleManagerOpenExistingFileTests
{
    private readonly FileManagementTestCompositionRoot _compositionRoot = new();

    [Trait("TestCaseId", "IT-FM-LC-OPEN-001")]
    [Fact]
    public void OpenFile_WhenFileExists_ShouldRegisterAndReturnOpenFileEntry()
    {
        // Arrange
        using var tempDir = new TemporaryDirectory();
        var filePath = Path.Combine(tempDir.Path, "test_lifecycle_open.db");
        var lifecycleManager = _compositionRoot.FileLifecycleManager;
        var physicalFs = _compositionRoot.PhysicalFileSystem;

        var initHandle = physicalFs.Create(filePath, 4096);
        physicalFs.Close(initHandle);

        // Act
        OpenFileEntry openEntry = null;
        try
        {
            openEntry = lifecycleManager.OpenFile(filePath, FileAccessMode.ReadWrite, FileLockMode.Exclusive);

            // Assert
            openEntry.Should().NotBeNull();
            openEntry.ReferenceCount.Should().Be(1);
        }
        finally
        {
            // Cleanup
            if (openEntry != null)
                lifecycleManager.CloseFile(filePath);
        }
    }
}
