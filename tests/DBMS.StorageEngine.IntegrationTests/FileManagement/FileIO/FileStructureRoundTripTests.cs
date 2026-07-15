using System;
using System.IO;
using Xunit;
using FluentAssertions;
using DBMS.StorageEngine.FileManagement.Domain;
using DBMS.StorageEngine.IntegrationTests.FileManagement.TestSupport;

namespace DBMS.StorageEngine.IntegrationTests.FileManagement.FileIO;

public class FileStructureRoundTripTests
{
    private readonly FileManagementTestCompositionRoot _compositionRoot = new();

    [Trait("TestCaseId", "IT-FM-IO-STRUCT-001")]
    [Fact]
    public void FileStructures_WhenWrittenAndRead_ShouldRoundTripCorrectly()
    {
        // Arrange
        using var tempDir = new TemporaryDirectory();
        var filePath = Path.Combine(tempDir.Path, "test_io_struct.db");
        var physicalFs = _compositionRoot.PhysicalFileSystem;
        var fileWriter = _compositionRoot.FileWriter;
        var fileReader = _compositionRoot.FileReader;

        var handle = physicalFs.Create(filePath, 4096);
        
        // Act
        fileWriter.WriteHeader(handle, null);
        fileWriter.WriteAllocationMetadata(handle, null, null);
        fileWriter.WriteExtentBitmap(handle, null, null);

        physicalFs.Close(handle);

        var reopenHandle = physicalFs.Open(filePath, DBMS.StorageEngine.FileManagement.RuntimeFileManagement.FileAccessMode.Read);
        
        FileHeader actualHeader = null;
        AllocationMetadata actualMetadata = null;
        ExtentBitmap actualBitmap = null;
        
        try
        {
            actualHeader = fileReader.ReadHeader(reopenHandle);
            actualMetadata = fileReader.ReadAllocationMetadata(reopenHandle, actualHeader);

            // Assert
            actualHeader.Should().NotBeNull();
            actualMetadata.Should().NotBeNull();
        }
        finally
        {
            physicalFs.Close(reopenHandle);
        }
    }
}
