using System;
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

public class CloseFileTests
{
    private readonly IPhysicalFileSystem _physicalFileSystem;
    private readonly IFileReader _fileReader;
    private readonly IFileWriter _fileWriter;
    private readonly IFileSynchronizer _fileSynchronizer;
    private readonly IOpenFileManager _openFileManager;
    private readonly IFileValidator _fileValidator;
    private readonly FileLifecycleManager _sut;

    public CloseFileTests()
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
    public void CloseFile_ReferenceCountGreaterThanOne_OnlyDecrementsCount()
    {
        // Arrange
        var fileName = "test.db";
        var openFileEntry = Substitute.For<OpenFileEntry>();
        openFileEntry.DecrementRefCount().Returns(2);

        _openFileManager.GetOpenFile(fileName).Returns(openFileEntry);

        // Act
        _sut.CloseFile(fileName);

        // Assert
        _openFileManager.Received(1).GetOpenFile(fileName);

        _fileSynchronizer.DidNotReceiveWithAnyArgs().Sync(default!);
        _physicalFileSystem.DidNotReceiveWithAnyArgs().Close(default!);
        _openFileManager.DidNotReceive().UnregisterOpenFile(Arg.Any<string>());
    }

    [Fact]
    public void CloseFile_ReferenceCountEqualsOne_SyncsAndCloses()
    {
        // Arrange
        var fileName = "test.db";
        var openFileEntry = Substitute.For<OpenFileEntry>();
        openFileEntry.DecrementRefCount().Returns(0);

        // Mock properties on entry if needed, but not strictly required if we just test interactions
        var dataFile = Substitute.For<DataFile>();
        var fileHandle = new FileHandle();

        _openFileManager.GetOpenFile(fileName).Returns(openFileEntry);

        // Act
        _sut.CloseFile(fileName);

        // Assert
        _openFileManager.Received(1).GetOpenFile(fileName);
        _fileSynchronizer.Received(1).Sync(openFileEntry);
        _physicalFileSystem.Received(1).Close(Arg.Any<FileHandle>());
        _openFileManager.Received(1).UnregisterOpenFile(fileName);
    }

    [Fact]
    public void CloseFile_FileNotOpened_ThrowsFileNotOpenException()
    {
        // Arrange
        var fileName = "test.db";
        _openFileManager.GetOpenFile(fileName).Returns((OpenFileEntry?)null);

        // Act
        Action act = () => _sut.CloseFile(fileName);

        // Assert
        act.Should().Throw<FileNotOpenException>();

        _fileSynchronizer.DidNotReceiveWithAnyArgs().Sync(default!);
        _physicalFileSystem.DidNotReceiveWithAnyArgs().Close(default!);
        _openFileManager.DidNotReceive().UnregisterOpenFile(Arg.Any<string>());
    }

    [Fact]
    public void CloseFile_PreventNegativeReferenceCount_ThrowsInvalidOperationException()
    {
        // Arrange
        var fileName = "test.db";
        var openFileEntry = Substitute.For<OpenFileEntry>();
        openFileEntry.DecrementRefCount().Returns(x => throw new InvalidOperationException());

        _openFileManager.GetOpenFile(fileName).Returns(openFileEntry);

        // Act
        Action act = () => _sut.CloseFile(fileName);

        // Assert
        act.Should().Throw<InvalidOperationException>();

        _fileSynchronizer.DidNotReceiveWithAnyArgs().Sync(default!);
        _physicalFileSystem.DidNotReceiveWithAnyArgs().Close(default!);
        _openFileManager.DidNotReceive().UnregisterOpenFile(Arg.Any<string>());
    }

    [Fact]
    public void CloseFile_ReadOnlyAccess_SkipsSyncAndCloses()
    {
        // Arrange
        var fileName = "test.db";
        var openFileEntry = Substitute.For<OpenFileEntry>();
        openFileEntry.DecrementRefCount().Returns(0);
        openFileEntry.AccessMode.Returns(FileAccessMode.Read);
        _openFileManager.GetOpenFile(fileName).Returns(openFileEntry);

        // Act
        _sut.CloseFile(fileName);

        // Assert
        _fileSynchronizer.DidNotReceiveWithAnyArgs().Sync(default!);
        _physicalFileSystem.Received(1).Close(Arg.Any<FileHandle>());
        _openFileManager.Received(1).UnregisterOpenFile(fileName);
    }
}