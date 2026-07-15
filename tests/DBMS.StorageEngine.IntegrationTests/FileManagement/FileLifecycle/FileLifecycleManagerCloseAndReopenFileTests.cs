using System;
using System.IO;
using Xunit;
using FluentAssertions;
using DBMS.StorageEngine.FileManagement.RuntimeFileManagement;
using DBMS.StorageEngine.IntegrationTests.FileManagement.TestSupport;

namespace DBMS.StorageEngine.IntegrationTests.FileManagement.FileLifecycle;

public class FileLifecycleManagerCloseAndReopenFileTests
{
    private readonly FileManagementTestCompositionRoot _compositionRoot = new();

    [Trait("TestCaseId", "IT-FM-LC-CLOSE-001")]
    [Fact]
    public void CloseAndReopenFile_WhenFileIsClosed_ShouldUnregisterAndAllowReopen()
    {
        // Arrange
        using var tempDir = new TemporaryDirectory();
        var filePath = Path.Combine(tempDir.Path, "test_lifecycle_close.db");
        var lifecycleManager = _compositionRoot.FileLifecycleManager;
        var physicalFs = _compositionRoot.PhysicalFileSystem;

        var initHandle = physicalFs.Create(filePath, 4096);
        physicalFs.Close(initHandle);

        var firstOpen = lifecycleManager.OpenFile(filePath, FileAccessMode.ReadWrite, FileLockMode.Exclusive);

        // Act
        lifecycleManager.CloseFile(filePath);
        var secondOpen = lifecycleManager.OpenFile(filePath, FileAccessMode.ReadWrite, FileLockMode.Exclusive);

        // Assert
        secondOpen.Should().NotBeNull();
        secondOpen.ReferenceCount.Should().Be(1);

        // Cleanup
        lifecycleManager.CloseFile(filePath);
    }
}
