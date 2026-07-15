using System;
using System.IO;
using Xunit;
using FluentAssertions;
using DBMS.StorageEngine.FileManagement.RuntimeFileManagement;
using DBMS.StorageEngine.IntegrationTests.FileManagement.TestSupport;
using DBMS.StorageEngine.FileManagement.Domain;

namespace DBMS.StorageEngine.IntegrationTests.FileManagement.FileIO;

public class FileReaderWriterPageTests
{
    private readonly FileManagementTestCompositionRoot _compositionRoot = new();

    [Trait("TestCaseId", "IT-FM-IO-RW-002")]
    [Fact]
    public void ReadWritePage_WhenValid_ShouldRoundTripBytes()
    {
        // Arrange
        using var tempDir = new TemporaryDirectory();
        var filePath = Path.Combine(tempDir.Path, "test_io_rw_page.db");
        var lifecycleManager = _compositionRoot.FileLifecycleManager;
        var physicalFs = _compositionRoot.PhysicalFileSystem;
        var fileWriter = _compositionRoot.FileWriter;
        var fileReader = _compositionRoot.FileReader;

        // Ensure we create a file large enough to hold a page at some offset
        var initHandle = physicalFs.Create(filePath, 1024 * 1024); // 1 MB
        physicalFs.Close(initHandle);

        OpenFileEntry openEntry = null;
        try
        {
            // Note: If FileLifecycleManager.OpenFile is throwing NotImplementedException, this will fail.
            // But this is part of Phase 4 (Test updates), Phase 5 will be implementing them through TDD.
            // Wait, we are supposed to update the tests so that they CAN run, we don't care if they pass yet since
            // FileLifecycleManager is unimplemented.
            openEntry = lifecycleManager.OpenFile(filePath, FileAccessMode.ReadWrite, FileLockMode.Exclusive);

            var expectedBytes = new byte[8192];
            new Random().NextBytes(expectedBytes); // Fill with random data
            
            var actualBytes = new byte[8192];

            // Act
            fileWriter.WritePage(openEntry, new PageId(1), expectedBytes);
            
            lifecycleManager.CloseFile(filePath);
            openEntry = lifecycleManager.OpenFile(filePath, FileAccessMode.ReadWrite, FileLockMode.Exclusive);

            fileReader.ReadPage(openEntry, new PageId(1), actualBytes);

            // Assert
            actualBytes.Should().Equal(expectedBytes);
        }
        catch (NotImplementedException)
        {
            // Expected during Phase 4 before Phase 5 implementation. We just want to ensure it compiles.
            // But usually tests should fail if NotImplemented, so we shouldn't catch it unless we want to bypass.
            // Let's just let it throw so it shows up as a Red test in TDD.
            throw;
        }
        finally
        {
            if (openEntry != null)
                lifecycleManager.CloseFile(filePath);
        }
    }
}
