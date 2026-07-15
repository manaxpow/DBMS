using System;
using Xunit;
using FluentAssertions;
using Moq;
using DBMS.StorageEngine.FileManagement.Domain;
using DBMS.StorageEngine.FileManagement.Exceptions;
using DBMS.StorageEngine.FileManagement.FileIO;
using DBMS.StorageEngine.FileManagement.PhysicalStorage;
using DBMS.StorageEngine.FileManagement.RuntimeFileManagement;

namespace DBMS.StorageEngine.UnitTests.FileManagement.FileIO;

public class FileWriterWritePageTests
{
    private readonly Mock<FileHandle> _fileHandleMock;
    private readonly OpenFileEntry _entry;
    private readonly FileWriter _sut;

    public FileWriterWritePageTests()
    {
        _fileHandleMock = new Mock<FileHandle>();
        
        var header = FileHeader.Create(new FileHeaderConfiguration(
            FileId: new FileId(),
            FileType: FileType.Data,
            PageSize: 4096,
            ExtentSize: 65536,
            FormatVersion: 1,
            HeaderSize: 8192,
            DataRegionOffset: 8192));
            
        var metadata = AllocationMetadata.Create(65536, 16);
        
        var dataFile = DataFile.Reconstruct(
            fileName: new FilePath("test.db"),
            header: header,
            metadata: metadata,
            currentSize: 1048576, // 1MB physical size
            maximumSize: null,
            autoExtendEnabled: false);

        _entry = new OpenFileEntry(
            dataFile,
            _fileHandleMock.Object,
            FileAccessMode.ReadWrite,
            FileLockMode.Exclusive);

        _sut = new FileWriter();
    }

    [Trait("TestCaseId", "UT-FM-IO-WRITE-PAGE-001")]
    [Fact]
    public void WritePage_WhenValidWithinBounds_ShouldWriteAtCorrectOffset()
    {
        // Arrange
        var pageId = new PageId(1);
        var source = new byte[4096];

        // Act
        _sut.WritePage(_entry, pageId, source);

        // Assert
        _fileHandleMock.Verify(h => h.WriteAtOffset(12288, It.IsAny<ReadOnlyMemory<byte>>()), Times.Once);
    }

    [Trait("TestCaseId", "UT-FM-IO-WRITE-PAGE-002")]
    [Fact]
    public void WritePage_WhenPageZero_ShouldWriteAtHeaderSizeOffset()
    {
        // Arrange
        var pageId = new PageId(0);
        var source = new byte[4096];

        // Act
        _sut.WritePage(_entry, pageId, source);

        // Assert
        _fileHandleMock.Verify(h => h.WriteAtOffset(8192, It.IsAny<ReadOnlyMemory<byte>>()), Times.Once);
    }

    [Trait("TestCaseId", "UT-FM-IO-WRITE-PAGE-003")]
    [Fact]
    public void WritePage_WhenReadOnlyAccess_ShouldThrowReadOnlyFileException()
    {
        // Arrange
        var readOnlyEntry = new OpenFileEntry(
            _entry.DataFile,
            _fileHandleMock.Object,
            FileAccessMode.Read,
            FileLockMode.Shared);

        var pageId = new PageId(1);
        var source = new byte[4096];

        // Act
        Action act = () => _sut.WritePage(readOnlyEntry, pageId, source);

        // Assert
        act.Should().Throw<ReadOnlyFileException>();
        _fileHandleMock.Verify(h => h.WriteAtOffset(It.IsAny<long>(), It.IsAny<ReadOnlyMemory<byte>>()), Times.Never);
    }

    [Trait("TestCaseId", "UT-FM-IO-WRITE-PAGE-004")]
    [Fact]
    public void WritePage_WhenNegativePageId_ShouldThrowInvalidPageIdException()
    {
        // Arrange
        var pageId = new PageId(-1);
        var source = new byte[4096];

        // Act
        Action act = () => _sut.WritePage(_entry, pageId, source);

        // Assert
        act.Should().Throw<InvalidPageIdException>();
        _fileHandleMock.Verify(h => h.WriteAtOffset(It.IsAny<long>(), It.IsAny<ReadOnlyMemory<byte>>()), Times.Never);
    }

    [Trait("TestCaseId", "UT-FM-IO-WRITE-PAGE-005")]
    [Fact]
    public void WritePage_WhenPageIdExceedsPhysicalBounds_ShouldThrowInvalidPageIdException()
    {
        // Arrange
        var pageId = new PageId(500); // Exceeds 1MB
        var source = new byte[4096];

        // Act
        Action act = () => _sut.WritePage(_entry, pageId, source);

        // Assert
        act.Should().Throw<InvalidPageIdException>();
        _fileHandleMock.Verify(h => h.WriteAtOffset(It.IsAny<long>(), It.IsAny<ReadOnlyMemory<byte>>()), Times.Never);
    }

    [Trait("TestCaseId", "UT-FM-IO-WRITE-PAGE-006")]
    [Fact]
    public void WritePage_WhenBufferSizeMismatchesPageSize_ShouldThrowArgumentException()
    {
        // Arrange
        var pageId = new PageId(1);
        var source = new byte[8192];

        // Act
        Action act = () => _sut.WritePage(_entry, pageId, source);

        // Assert
        act.Should().Throw<ArgumentException>();
        _fileHandleMock.Verify(h => h.WriteAtOffset(It.IsAny<long>(), It.IsAny<ReadOnlyMemory<byte>>()), Times.Never);
    }

    [Trait("TestCaseId", "UT-FM-IO-WRITE-PAGE-007")]
    [Fact]
    public void WritePage_WhenUnderlyingWriteFails_ShouldPropagateException()
    {
        // Arrange
        var pageId = new PageId(1);
        var source = new byte[4096];
        
        _fileHandleMock.Setup(h => h.WriteAtOffset(12288, It.IsAny<ReadOnlyMemory<byte>>()))
            .Throws(new WriteFailureException("OS write error"));

        // Act
        Action act = () => _sut.WritePage(_entry, pageId, source);

        // Assert
        act.Should().Throw<WriteFailureException>();
    }
}
