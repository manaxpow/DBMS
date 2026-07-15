using System;
using System.IO;
using Xunit;
using FluentAssertions;
using NSubstitute;
using DBMS.StorageEngine.FileManagement.Domain;
using DBMS.StorageEngine.FileManagement.Exceptions;
using DBMS.StorageEngine.FileManagement.FileIO;
using DBMS.StorageEngine.FileManagement.FileLifecycle;
using DBMS.StorageEngine.FileManagement.PhysicalStorage;
using DBMS.StorageEngine.FileManagement.RuntimeFileManagement;

namespace DBMS.StorageEngine.UnitTests.FileManagement.FileLifecycle;

public class ResizeFileTests
{
    private readonly IPhysicalFileSystem _physicalFileSystem;
    private readonly IFileReader _fileReader;
    private readonly IFileWriter _fileWriter;
    private readonly IFileSynchronizer _fileSynchronizer;
    private readonly IOpenFileManager _openFileManager;
    private readonly IFileValidator _fileValidator;
    private readonly FileLifecycleManager _sut;

    public ResizeFileTests()
    {
        _physicalFileSystem = Substitute.For<IPhysicalFileSystem>();
        _fileReader = Substitute.For<IFileReader>();
        _fileWriter = Substitute.For<IFileWriter>();
        _fileSynchronizer = Substitute.For<IFileSynchronizer>();
        _openFileManager = Substitute.For<IOpenFileManager>();
        _fileValidator = Substitute.For<IFileValidator>();

        _sut = new FileLifecycleManager(
            _physicalFileSystem,
            _fileReader,
            _fileWriter,
            _fileSynchronizer,
            _openFileManager,
            _fileValidator);
    }

    [Fact]
    public void ResizeFile_ExtendFileSuccessfully_UpdatesPhysicalAndMetadata()
    {
        // Arrange
        var entry = Substitute.For<OpenFileEntry>();
        var dataFile = Substitute.For<DataFile>();
        var metadata = Substitute.For<AllocationMetadata>();
        
        dataFile.CurrentSize.Returns(1048576L); // 1MB
        dataFile.AllocationMetadata.Returns(metadata);
        entry.DataFile.Returns(dataFile);
        
        long newSize = 2097152L; // 2MB

        // Act
        _sut.ResizeFile(entry, newSize);

        // Assert
        _physicalFileSystem.Received(1).Resize(Arg.Any<FileHandle>(), newSize);
        metadata.Received(1).AddExtents(Arg.Any<int>());
        _fileWriter.Received(1).WriteAllocationMetadata(Arg.Any<FileHandle>(), Arg.Any<FileHeader>(), metadata);
        _fileSynchronizer.Received(1).Sync(entry);
    }

    [Fact]
    public void ResizeFile_TruncateFileSafely_ShrinksAndUpdatesMetadata()
    {
        // Arrange
        var entry = Substitute.For<OpenFileEntry>();
        var dataFile = Substitute.For<DataFile>();
        var metadata = Substitute.For<AllocationMetadata>();
        
        dataFile.CurrentSize.Returns(2097152L); // 2MB
        dataFile.AllocationMetadata.Returns(metadata);
        entry.DataFile.Returns(dataFile);

        metadata.CanTruncateTo(Arg.Any<int>()).Returns(true);
        
        long newSize = 1048576L; // 1MB

        // Act
        _sut.ResizeFile(entry, newSize);

        // Assert
        metadata.Received(1).TruncateTo(Arg.Any<int>());
        _fileWriter.Received(1).WriteAllocationMetadata(Arg.Any<FileHandle>(), Arg.Any<FileHeader>(), metadata);
        _physicalFileSystem.Received(1).Resize(Arg.Any<FileHandle>(), newSize);
        _fileSynchronizer.Received(1).Sync(entry);
    }

    [Fact]
    public void ResizeFile_ResizeNoOp_DoesNothing()
    {
        // Arrange
        var entry = Substitute.For<OpenFileEntry>();
        var dataFile = Substitute.For<DataFile>();
        
        dataFile.CurrentSize.Returns(1048576L); // 1MB
        entry.DataFile.Returns(dataFile);
        
        long newSize = 1048576L; // 1MB

        // Act
        _sut.ResizeFile(entry, newSize);

        // Assert
        _physicalFileSystem.DidNotReceiveWithAnyArgs().Resize(default!, default!);
        _fileWriter.DidNotReceiveWithAnyArgs().WriteAllocationMetadata(default!, default!, default!);
        _fileSynchronizer.DidNotReceiveWithAnyArgs().Sync(default!);
    }

    [Theory]
    [InlineData(-500)]
    [InlineData(1044481)] // Not aligned
    public void ResizeFile_InvalidSizeArguments_ThrowsArgumentOutOfRangeException(long newSize)
    {
        // Arrange
        var entry = Substitute.For<OpenFileEntry>();

        // Act
        Action act = () => _sut.ResizeFile(entry, newSize);

        // Assert
        act.Should().Throw<ArgumentOutOfRangeException>();
        _physicalFileSystem.DidNotReceiveWithAnyArgs().Resize(default!, default!);
    }

    [Fact]
    public void ResizeFile_TruncateCutsIntoUsedExtent_ThrowsFileTruncationException()
    {
        // Arrange
        var entry = Substitute.For<OpenFileEntry>();
        var dataFile = Substitute.For<DataFile>();
        var metadata = Substitute.For<AllocationMetadata>();
        
        dataFile.CurrentSize.Returns(2097152L); // 2MB
        dataFile.AllocationMetadata.Returns(metadata);
        entry.DataFile.Returns(dataFile);

        metadata.CanTruncateTo(Arg.Any<int>()).Returns(false);
        
        long newSize = 1048576L; // 1MB

        // Act
        Action act = () => _sut.ResizeFile(entry, newSize);

        // Assert
        act.Should().Throw<FileTruncationException>();
        _physicalFileSystem.DidNotReceiveWithAnyArgs().Resize(default!, default!);
        _fileWriter.DidNotReceiveWithAnyArgs().WriteAllocationMetadata(default!, default!, default!);
    }

    [Fact]
    public void ResizeFile_MaximumFileSizeExceeded_ThrowsMaximumFileSizeExceededException()
    {
        // Arrange
        var entry = Substitute.For<OpenFileEntry>();
        var dataFile = Substitute.For<DataFile>();
        
        dataFile.CurrentSize.Returns(1048576L); // 1MB
        dataFile.MaximumSize.Returns(10485760L); // 10MB
        entry.DataFile.Returns(dataFile);
        
        long newSize = 20971520L; // 20MB

        // Act
        Action act = () => _sut.ResizeFile(entry, newSize);

        // Assert
        act.Should().Throw<MaximumFileSizeExceededException>();
    }

    [Fact]
    public void ResizeFile_PhysicalOSResizeFails_ThrowsFileResizeException()
    {
        // Arrange
        var entry = Substitute.For<OpenFileEntry>();
        var dataFile = Substitute.For<DataFile>();
        var metadata = Substitute.For<AllocationMetadata>();
        
        dataFile.CurrentSize.Returns(1048576L); // 1MB
        dataFile.AllocationMetadata.Returns(metadata);
        entry.DataFile.Returns(dataFile);
        
        long newSize = 2097152L; // 2MB

        _physicalFileSystem.When(x => x.Resize(Arg.Any<FileHandle>(), newSize))
            .Throw(new IOException("Disk full"));

        // Act
        Action act = () => _sut.ResizeFile(entry, newSize);

        // Assert
        act.Should().Throw<FileResizeException>()
            .WithInnerException<IOException>();

        _physicalFileSystem.Received(1).Resize(Arg.Any<FileHandle>(), newSize);
        _fileWriter.DidNotReceiveWithAnyArgs().WriteAllocationMetadata(default!, default!, default!);
    }
}
