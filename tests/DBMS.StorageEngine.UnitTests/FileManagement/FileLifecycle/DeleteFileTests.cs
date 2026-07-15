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

public class DeleteFileTests
{
    private readonly IPhysicalFileSystem _physicalFileSystem;
    private readonly IFileReader _fileReader;
    private readonly IFileWriter _fileWriter;
    private readonly IFileSynchronizer _fileSynchronizer;
    private readonly IOpenFileManager _openFileManager;
    private readonly IFileValidator _fileValidator;
    private readonly FileLifecycleManager _sut;

    public DeleteFileTests()
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
    public void DeleteFile_SuccessfulDeletion_DeletesPhysicallyAndCompletesInRegistry()
    {
        // Arrange
        var fileName = "test.db";
        _physicalFileSystem.Exists(fileName).Returns(true);
        _openFileManager.TryBeginDelete(fileName).Returns(true);

        // Act
        _sut.DeleteFile(fileName);

        // Assert
        _physicalFileSystem.Received(1).Exists(fileName);
        _openFileManager.Received(1).TryBeginDelete(fileName);
        _physicalFileSystem.Received(1).Delete(fileName);
        _openFileManager.Received(1).CompleteDelete(fileName);

        _openFileManager.DidNotReceiveWithAnyArgs().CancelDelete(default!);
    }

    [Fact]
    public void DeleteFile_BlockedWhenFileIsOpen_ThrowsFileInUseException()
    {
        // Arrange
        var fileName = "test.db";
        _physicalFileSystem.Exists(fileName).Returns(true);
        _openFileManager.TryBeginDelete(fileName).Returns(false);

        // Act
        Action act = () => _sut.DeleteFile(fileName);

        // Assert
        act.Should().Throw<FileInUseException>();

        _physicalFileSystem.Received(1).Exists(fileName);
        _openFileManager.Received(1).TryBeginDelete(fileName);
        _physicalFileSystem.DidNotReceiveWithAnyArgs().Delete(default!);
        _openFileManager.DidNotReceiveWithAnyArgs().CompleteDelete(default!);
        _openFileManager.DidNotReceiveWithAnyArgs().CancelDelete(default!);
    }

    [Fact]
    public void DeleteFile_FileDoesNotExist_ThrowsFileNotFoundException()
    {
        // Arrange
        var fileName = "missing.db";
        _physicalFileSystem.Exists(fileName).Returns(false);

        // Act
        Action act = () => _sut.DeleteFile(fileName);

        // Assert
        act.Should().Throw<FileNotFoundException>();

        _physicalFileSystem.Received(1).Exists(fileName);
        _openFileManager.DidNotReceiveWithAnyArgs().TryBeginDelete(default!);
        _physicalFileSystem.DidNotReceiveWithAnyArgs().Delete(default!);
    }

    [Fact]
    public void DeleteFile_PhysicalDeleteFails_CancelsDeleteAndThrowsException()
    {
        // Arrange
        var fileName = "test.db";
        _physicalFileSystem.Exists(fileName).Returns(true);
        _openFileManager.TryBeginDelete(fileName).Returns(true);
        _physicalFileSystem.When(x => x.Delete(fileName)).Throw(new IOException("Disk read only"));

        // Act
        Action act = () => _sut.DeleteFile(fileName);

        // Assert
        act.Should().Throw<IOException>();

        _physicalFileSystem.Received(1).Exists(fileName);
        _openFileManager.Received(1).TryBeginDelete(fileName);
        _physicalFileSystem.Received(1).Delete(fileName);
        _openFileManager.Received(1).CancelDelete(fileName);

        _openFileManager.DidNotReceiveWithAnyArgs().CompleteDelete(default!);
    }
}
