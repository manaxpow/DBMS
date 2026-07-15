using System;
using System.IO;
using Xunit;
using FluentAssertions;
using DBMS.StorageEngine.FileManagement.RuntimeFileManagement;
using DBMS.StorageEngine.IntegrationTests.FileManagement.TestSupport;

namespace DBMS.StorageEngine.IntegrationTests.FileManagement.FileLifecycle;

public class FileLifecycleManagerDeleteFileTests
{
    private readonly FileManagementTestCompositionRoot _compositionRoot = new();

    [Trait("TestCaseId", "IT-FM-LC-DELETE-001")]
    [Fact]
    public void DeleteFile_WhenFileIsClosed_ShouldRemoveFileAndUnregister()
    {
        // Arrange
        using var tempDir = new TemporaryDirectory();
        var filePath = Path.Combine(tempDir.Path, "test_lifecycle_delete.db");
        var lifecycleManager = _compositionRoot.FileLifecycleManager;
        var physicalFs = _compositionRoot.PhysicalFileSystem;

        var handle = physicalFs.Create(filePath, 4096);
        physicalFs.Close(handle);

        // Act
        lifecycleManager.DeleteFile(filePath);

        // Assert
        physicalFs.Exists(filePath).Should().BeFalse();
        
        Action openAction = () => lifecycleManager.OpenFile(filePath, FileAccessMode.ReadWrite, FileLockMode.Exclusive);
        openAction.Should().Throw<Exception>(); // Should be FileNotFoundException or similar from OS/Lifecycle
    }
}
