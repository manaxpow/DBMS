using System;
using System.IO;
using Xunit;
using FluentAssertions;
using DBMS.StorageEngine.FileManagement.PhysicalStorage;
using DBMS.StorageEngine.FileManagement.RuntimeFileManagement;
using DBMS.StorageEngine.FileManagement.Exceptions;
using DBMS.StorageEngine.IntegrationTests.FileManagement.TestSupport;

namespace DBMS.StorageEngine.IntegrationTests.FileManagement.PhysicalStorage;

public class PhysicalFileSystemTests
{
    private readonly FileManagementTestCompositionRoot _compositionRoot = new();

    [Trait("TestCaseId", "IT-FM-PS-001")]
    [Fact]
    public void CreateFile_WhenPathIsValid_ShouldCreateEmptyPhysicalFile()
    {
        // Arrange
        using var tempDir = new TemporaryDirectory();
        var filePath = Path.Combine(tempDir.Path, "test_create.db");
        var physicalFs = _compositionRoot.PhysicalFileSystem;

        // Act
        FileHandle handle = null;
        try
        {
            handle = physicalFs.Create(filePath, 4096);
            
            // Assert
            physicalFs.Exists(filePath).Should().BeTrue();
            physicalFs.GetSize(handle).Should().Be(4096);
        }
        finally
        {
            // Cleanup
            if (handle != null)
                physicalFs.Close(handle);
        }
    }

    [Trait("TestCaseId", "IT-FM-PS-002")]
    [Fact]
    public void OpenFile_WhenAlreadyOpenExclusively_ShouldFailOrBlockSecondaryAccess()
    {
        // Arrange
        using var tempDir = new TemporaryDirectory();
        var filePath = Path.Combine(tempDir.Path, "test_exclusive.db");
        var physicalFs = _compositionRoot.PhysicalFileSystem;
        
        FileHandle handleA = null;
        try
        {
            // First we need to create it (since Open requires it to exist or we use Create, but the spec says "Thread A calls PhysicalFileSystem.Open"). Let's create it first.
            var initHandle = physicalFs.Create(filePath, 4096);
            physicalFs.Close(initHandle);

            handleA = physicalFs.Open(filePath, FileAccessMode.ReadWrite);

            // Act
            Action act = () => physicalFs.Open(filePath, FileAccessMode.ReadWrite);

            // Assert
            act.Should().Throw<IOException>(); // OS-level sharing violation
        }
        finally
        {
            // Cleanup
            if (handleA != null)
                physicalFs.Close(handleA);
        }
    }

    [Trait("TestCaseId", "IT-FM-PS-003")]
    [Fact]
    public void GetSize_WhenCalledOnOpenHandle_ShouldReturnCorrectPhysicalSize()
    {
        // Arrange
        using var tempDir = new TemporaryDirectory();
        var filePath = Path.Combine(tempDir.Path, "test_getsize.db");
        var physicalFs = _compositionRoot.PhysicalFileSystem;

        FileHandle handle = null;
        try
        {
            handle = physicalFs.Create(filePath, 4096);
            
            // Act
            long size = physicalFs.GetSize(handle);

            // Assert
            size.Should().Be(4096);
        }
        finally
        {
            if (handle != null)
                physicalFs.Close(handle);
        }
    }

    [Trait("TestCaseId", "IT-FM-PS-004")]
    [Fact]
    public void ResizeFile_WhenRequestedSizeIsLarger_ShouldExtendPhysicalFile()
    {
        // Arrange
        using var tempDir = new TemporaryDirectory();
        var filePath = Path.Combine(tempDir.Path, "test_resize.db");
        var physicalFs = _compositionRoot.PhysicalFileSystem;

        FileHandle handle = null;
        try
        {
            handle = physicalFs.Create(filePath, 4096);
            
            // Act
            physicalFs.Resize(handle, 8192);

            // Assert
            physicalFs.GetSize(handle).Should().Be(8192);
        }
        finally
        {
            if (handle != null)
                physicalFs.Close(handle);
        }
    }

    [Trait("TestCaseId", "IT-FM-PS-005")]
    [Fact]
    public void DeleteFile_WhenFileExists_ShouldRemovePhysicalFileFromDisk()
    {
        // Arrange
        using var tempDir = new TemporaryDirectory();
        var filePath = Path.Combine(tempDir.Path, "test_delete.db");
        var physicalFs = _compositionRoot.PhysicalFileSystem;

        var handle = physicalFs.Create(filePath, 4096);
        physicalFs.Close(handle);
        physicalFs.Exists(filePath).Should().BeTrue();

        // Act
        physicalFs.Delete(filePath);

        // Assert
        physicalFs.Exists(filePath).Should().BeFalse();
    }
}
