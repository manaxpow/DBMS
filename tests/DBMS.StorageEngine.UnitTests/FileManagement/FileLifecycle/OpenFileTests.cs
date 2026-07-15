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
using FileNotFoundException = DBMS.StorageEngine.FileManagement.Exceptions.FileNotFoundException;

namespace DBMS.StorageEngine.UnitTests.FileManagement.FileLifecycle;

public class OpenFileTests
{
    private readonly IPhysicalFileSystem _physicalFileSystem;
    private readonly IFileReader _fileReader;
    private readonly IFileWriter _fileWriter;
    private readonly IFileSynchronizer _fileSynchronizer;
    private readonly IOpenFileManager _openFileManager;
    private readonly IFileValidator _fileValidator;
    private readonly FileLifecycleManager _sut;

    public OpenFileTests()
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
    public void OpenFile_FirstTimeOpenSuccess_RegistersEntry()
    {
        // Arrange
        var fileName = "test.db";
        var accessMode = FileAccessMode.ReadWrite;
        var lockMode = FileLockMode.Exclusive;

        _openFileManager.GetOpenFile(fileName).Returns((OpenFileEntry?)null);
        _physicalFileSystem.Exists(fileName).Returns(true);
        var fileHandle = new FileHandle();
        _physicalFileSystem.Open(fileName, accessMode).Returns(fileHandle);
        
        var header = new FileHeader();
        var metadata = new AllocationMetadata();
        var bitmap = new ExtentBitmap();
        _fileReader.ReadHeader(fileHandle).Returns(header);
        _fileReader.ReadAllocationMetadata(fileHandle, header).Returns(metadata);

        _physicalFileSystem.GetSize(fileHandle).Returns(1048576L);

        // Act
        var result = _sut.OpenFile(fileName, accessMode, lockMode);

        // Assert
        _openFileManager.Received(1).GetOpenFile(fileName);
        _physicalFileSystem.Received(1).Exists(fileName);
        _physicalFileSystem.Received(1).Open(fileName, accessMode);
        _fileReader.Received(1).ReadHeader(fileHandle);
        _fileReader.Received(1).ReadAllocationMetadata(fileHandle, header);

        _physicalFileSystem.Received(1).GetSize(fileHandle);
        _fileValidator.Received(1).Validate(header, metadata, bitmap, 1048576L);
        _openFileManager.Received(1).RegisterOpenFile(fileName, Arg.Any<OpenFileEntry>());
    }

    [Fact]
    public void OpenFile_ReopenCompatible_IncrementsReferenceCount()
    {
        // Arrange
        var fileName = "test.db";
        var accessMode = FileAccessMode.Read;
        var lockMode = FileLockMode.Shared;

        var existingEntry = Substitute.For<OpenFileEntry>();
        existingEntry.AccessMode.Returns(FileAccessMode.Read);
        existingEntry.LockMode.Returns(FileLockMode.Shared);

        _openFileManager.GetOpenFile(fileName).Returns(existingEntry);

        // Act
        var result = _sut.OpenFile(fileName, accessMode, lockMode);

        // Assert
        result.Should().BeSameAs(existingEntry);
        
        _physicalFileSystem.DidNotReceiveWithAnyArgs().Open(default!, default!);
        _fileReader.DidNotReceiveWithAnyArgs().ReadHeader(default!);
        _fileValidator.DidNotReceiveWithAnyArgs().Validate(default!, default!, default!, default!);
    }

    [Fact]
    public void OpenFile_ExclusiveLockConflict_ThrowsLockConflictException()
    {
        // Arrange
        var fileName = "test.db";
        var accessMode = FileAccessMode.ReadWrite;
        var lockMode = FileLockMode.Exclusive;

        var existingEntry = Substitute.For<OpenFileEntry>();
        existingEntry.LockMode.Returns(FileLockMode.Shared);

        _openFileManager.GetOpenFile(fileName).Returns(existingEntry);

        // Act
        Action act = () => _sut.OpenFile(fileName, accessMode, lockMode);

        // Assert
        act.Should().Throw<LockConflictException>();
        _physicalFileSystem.DidNotReceiveWithAnyArgs().Open(default!, default!);
    }

    [Fact]
    public void OpenFile_AccessModeConflict_ThrowsLockConflictException()
    {
        // Arrange
        var fileName = "test.db";
        var accessMode = FileAccessMode.ReadWrite;
        var lockMode = FileLockMode.Shared;

        var existingEntry = Substitute.For<OpenFileEntry>();
        existingEntry.AccessMode.Returns(FileAccessMode.Read);

        _openFileManager.GetOpenFile(fileName).Returns(existingEntry);

        // Act
        Action act = () => _sut.OpenFile(fileName, accessMode, lockMode);

        // Assert
        act.Should().Throw<LockConflictException>();
        _physicalFileSystem.DidNotReceiveWithAnyArgs().Open(default!, default!);
    }

    [Fact]
    public void OpenFile_FileDoesNotExist_ThrowsFileNotFoundException()
    {
        // Arrange
        var fileName = "missing.db";
        _openFileManager.GetOpenFile(fileName).Returns((OpenFileEntry?)null);
        _physicalFileSystem.Exists(fileName).Returns(false);

        // Act
        Action act = () => _sut.OpenFile(fileName, FileAccessMode.Read, FileLockMode.Shared);

        // Assert
        act.Should().Throw<FileNotFoundException>();
        _physicalFileSystem.DidNotReceiveWithAnyArgs().Open(default!, default!);
    }

    [Fact]
    public void OpenFile_OSOpenHandleFails_ThrowsFileOpenException()
    {
        // Arrange
        var fileName = "test.db";
        _openFileManager.GetOpenFile(fileName).Returns((OpenFileEntry?)null);
        _physicalFileSystem.Exists(fileName).Returns(true);
        _physicalFileSystem.Open(fileName, Arg.Any<FileAccessMode>()).Returns(x => throw new IOException("Locked"));

        // Act
        Action act = () => _sut.OpenFile(fileName, FileAccessMode.Read, FileLockMode.Shared);

        // Assert
        act.Should().Throw<FileOpenException>()
            .WithInnerException<IOException>();

        _fileReader.DidNotReceiveWithAnyArgs().ReadHeader(default!);
        _openFileManager.DidNotReceiveWithAnyArgs().RegisterOpenFile(default!, default!);
    }

    [Fact]
    public void OpenFile_ReadHeaderFails_ClosesHandleAndThrowsFileOpenException()
    {
        // Arrange
        var fileName = "test.db";
        var fileHandle = new FileHandle();
        _openFileManager.GetOpenFile(fileName).Returns((OpenFileEntry?)null);
        _physicalFileSystem.Exists(fileName).Returns(true);
        _physicalFileSystem.Open(fileName, Arg.Any<FileAccessMode>()).Returns(fileHandle);
        _fileReader.ReadHeader(fileHandle).Returns(x => throw new IOException());

        // Act
        Action act = () => _sut.OpenFile(fileName, FileAccessMode.Read, FileLockMode.Shared);

        // Assert
        act.Should().Throw<FileOpenException>();
        _physicalFileSystem.Received(1).Close(fileHandle);
        _openFileManager.DidNotReceiveWithAnyArgs().RegisterOpenFile(default!, default!);
    }

    [Fact]
    public void OpenFile_ReadMetadataFails_ClosesHandleAndThrowsFileOpenException()
    {
        // Arrange
        var fileName = "test.db";
        var fileHandle = new FileHandle();
        _openFileManager.GetOpenFile(fileName).Returns((OpenFileEntry?)null);
        _physicalFileSystem.Exists(fileName).Returns(true);
        _physicalFileSystem.Open(fileName, Arg.Any<FileAccessMode>()).Returns(fileHandle);
        var header = new FileHeader();
        _fileReader.ReadHeader(fileHandle).Returns(header);
        _fileReader.ReadAllocationMetadata(fileHandle, header).Returns(x => throw new IOException());

        // Act
        Action act = () => _sut.OpenFile(fileName, FileAccessMode.Read, FileLockMode.Shared);

        // Assert
        act.Should().Throw<FileOpenException>();
        _physicalFileSystem.Received(1).Close(fileHandle);
        _openFileManager.DidNotReceiveWithAnyArgs().RegisterOpenFile(default!, default!);
    }

    [Fact]
    public void OpenFile_ValidationFails_ClosesHandleAndThrowsInvalidFileFormatException()
    {
        // Arrange
        var fileName = "test.db";
        var fileHandle = new FileHandle();
        _openFileManager.GetOpenFile(fileName).Returns((OpenFileEntry?)null);
        _physicalFileSystem.Exists(fileName).Returns(true);
        _physicalFileSystem.Open(fileName, Arg.Any<FileAccessMode>()).Returns(fileHandle);
        
        var header = new FileHeader();
        var metadata = new AllocationMetadata();
        var bitmap = new ExtentBitmap();
        _fileReader.ReadHeader(fileHandle).Returns(header);
        _fileReader.ReadAllocationMetadata(fileHandle, header).Returns(metadata);

        _physicalFileSystem.GetSize(fileHandle).Returns(1048576L);

        _fileValidator.When(x => x.Validate(header, metadata, bitmap, 1048576L))
            .Throw(new InvalidFileFormatException("Bad magic number"));

        // Act
        Action act = () => _sut.OpenFile(fileName, FileAccessMode.Read, FileLockMode.Shared);

        // Assert
        act.Should().Throw<InvalidFileFormatException>();
        _physicalFileSystem.Received(1).Close(fileHandle);
        _openFileManager.DidNotReceiveWithAnyArgs().RegisterOpenFile(default!, default!);
    }
}
