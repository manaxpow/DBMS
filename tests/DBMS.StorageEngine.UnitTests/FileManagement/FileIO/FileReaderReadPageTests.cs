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

public class FileReaderReadPageTests
{
    private readonly Mock<FileHandle> _fileHandleMock;
    private readonly OpenFileEntry _entry;
    private readonly FileReader _sut;

    public FileReaderReadPageTests()
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

        _sut = new FileReader();
    }

    [Trait("TestCaseId", "UT-FM-IO-READ-PAGE-001")]
    [Fact]
    public void ReadPage_WhenValidWithinBounds_ShouldReadAtCorrectOffset()
    {
        // Arrange
        var pageId = new PageId(1);
        var destination = new byte[4096];
        
        _fileHandleMock.Setup(h => h.ReadAtOffset(12288, It.IsAny<Memory<byte>>()))
            .Returns(4096)
            .Callback<long, Memory<byte>>((offset, dest) => 
            {
                var span = dest.Span;
                span[0] = 42;
            });

        // Act
        _sut.ReadPage(_entry, pageId, destination);

        // Assert
        destination[0].Should().Be(42);
        _fileHandleMock.Verify(h => h.ReadAtOffset(12288, It.IsAny<Memory<byte>>()), Times.Once);
    }

    [Trait("TestCaseId", "UT-FM-IO-READ-PAGE-002")]
    [Fact]
    public void ReadPage_WhenPageZero_ShouldReadAtHeaderSizeOffset()
    {
        // Arrange
        var pageId = new PageId(0);
        var destination = new byte[4096];
        
        _fileHandleMock.Setup(h => h.ReadAtOffset(8192, It.IsAny<Memory<byte>>()))
            .Returns(4096);

        // Act
        _sut.ReadPage(_entry, pageId, destination);

        // Assert
        _fileHandleMock.Verify(h => h.ReadAtOffset(8192, It.IsAny<Memory<byte>>()), Times.Once);
    }

    [Trait("TestCaseId", "UT-FM-IO-READ-PAGE-003")]
    [Fact]
    public void ReadPage_WhenNegativePageId_ShouldThrowInvalidPageIdException()
    {
        // Arrange
        var pageId = new PageId(-1);
        var destination = new byte[4096];

        // Act
        Action act = () => _sut.ReadPage(_entry, pageId, destination);

        // Assert
        act.Should().Throw<InvalidPageIdException>();
        _fileHandleMock.Verify(h => h.ReadAtOffset(It.IsAny<long>(), It.IsAny<Memory<byte>>()), Times.Never);
    }

    [Trait("TestCaseId", "UT-FM-IO-READ-PAGE-004")]
    [Fact]
    public void ReadPage_WhenPageIdExceedsPhysicalBounds_ShouldThrowInvalidPageIdException()
    {
        // Arrange
        var pageId = new PageId(500); // Exceeds 1MB
        var destination = new byte[4096];

        // Act
        Action act = () => _sut.ReadPage(_entry, pageId, destination);

        // Assert
        act.Should().Throw<InvalidPageIdException>();
        _fileHandleMock.Verify(h => h.ReadAtOffset(It.IsAny<long>(), It.IsAny<Memory<byte>>()), Times.Never);
    }

    [Trait("TestCaseId", "UT-FM-IO-READ-PAGE-005")]
    [Fact]
    public void ReadPage_WhenBufferSizeMismatchesPageSize_ShouldThrowArgumentException()
    {
        // Arrange
        var pageId = new PageId(1);
        var destination = new byte[8192];

        // Act
        Action act = () => _sut.ReadPage(_entry, pageId, destination);

        // Assert
        act.Should().Throw<ArgumentException>();
        _fileHandleMock.Verify(h => h.ReadAtOffset(It.IsAny<long>(), It.IsAny<Memory<byte>>()), Times.Never);
    }

    [Trait("TestCaseId", "UT-FM-IO-READ-PAGE-006")]
    [Fact]
    public void ReadPage_WhenUnderlyingReadFails_ShouldPropagateException()
    {
        // Arrange
        var pageId = new PageId(1);
        var destination = new byte[4096];
        
        _fileHandleMock.Setup(h => h.ReadAtOffset(12288, It.IsAny<Memory<byte>>()))
            .Throws(new ReadFailureException("OS read error"));

        // Act
        Action act = () => _sut.ReadPage(_entry, pageId, destination);

        // Assert
        act.Should().Throw<ReadFailureException>();
    }
}
