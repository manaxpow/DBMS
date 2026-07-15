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

public class CreateFileTests
{
    private readonly IPhysicalFileSystem _physicalFileSystem;
    private readonly IFileReader _fileReader;
    private readonly IFileWriter _fileWriter;
    private readonly IFileSynchronizer _fileSynchronizer;
    private readonly IOpenFileManager _openFileManager;
    private readonly IFileValidator _fileValidator;
    private readonly FileLifecycleManager _sut;

    public CreateFileTests()
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
    public void CreateFile_ValidInputs_CreatesPhysicalFileAndWritesStructures()
    {
        // Arrange
        var fileName = "test.db";
        var fileType = FileType.Data;
        var pageSize = 4096;
        var initialFileSize = 1048576L;
        var fileHandle = new FileHandle();

        _physicalFileSystem.Exists(fileName).Returns(false);
        _physicalFileSystem.Create(fileName, initialFileSize).Returns(fileHandle);

        // Act
        var result = _sut.CreateFile(fileName, fileType, pageSize, initialFileSize);

        // Assert
        result.Should().NotBeNull();
        result.FileName.Should().Be(fileName);

        _physicalFileSystem.Received(1).Exists(fileName);
        _physicalFileSystem.Received(1).Create(fileName, initialFileSize);
        _fileWriter.Received(1).WriteHeader(fileHandle, Arg.Any<FileHeader>());
        _fileWriter.Received(1).WriteAllocationMetadata(fileHandle, Arg.Any<FileHeader>(), Arg.Any<AllocationMetadata>());
        _fileWriter.Received(1).WriteExtentBitmap(fileHandle, Arg.Any<FileHeader>(), Arg.Any<AllocationMetadata>());
        _physicalFileSystem.Received(1).Close(fileHandle);

        _physicalFileSystem.DidNotReceive().Delete(Arg.Any<string>());
    }

    [Fact]
    public void CreateFile_FileAlreadyExists_ThrowsFileAlreadyExistsException()
    {
        // Arrange
        var fileName = "test.db";
        _physicalFileSystem.Exists(fileName).Returns(true);

        // Act
        Action act = () => _sut.CreateFile(fileName, FileType.Data, 4096, 1048576);

        // Assert
        act.Should().Throw<FileAlreadyExistsException>();

        _physicalFileSystem.Received(1).Exists(fileName);
        _physicalFileSystem.DidNotReceive().Create(Arg.Any<string>(), Arg.Any<long>());
        _fileWriter.DidNotReceiveWithAnyArgs().WriteHeader(default!, default!);
        _fileWriter.DidNotReceiveWithAnyArgs().WriteAllocationMetadata(default!, default!, default!);
        _fileWriter.DidNotReceiveWithAnyArgs().WriteExtentBitmap(default!, default!, default!);
        _physicalFileSystem.DidNotReceiveWithAnyArgs().Close(default!);
        _physicalFileSystem.DidNotReceiveWithAnyArgs().Delete(default!);
    }

    [Fact]
    public void CreateFile_PhysicalCreationFails_ThrowsFileCreationException()
    {
        // Arrange
        var fileName = "test.db";
        _physicalFileSystem.Exists(fileName).Returns(false);
        _physicalFileSystem.Create(fileName, Arg.Any<long>()).Returns(x => throw new IOException("Disk error"));

        // Act
        Action act = () => _sut.CreateFile(fileName, FileType.Data, 4096, 1048576);

        // Assert
        act.Should().Throw<FileCreationException>()
            .WithInnerException<IOException>();

        _physicalFileSystem.Received(1).Create(fileName, 1048576);
        _fileWriter.DidNotReceiveWithAnyArgs().WriteHeader(default!, default!);
        _fileWriter.DidNotReceiveWithAnyArgs().WriteAllocationMetadata(default!, default!, default!);
        _fileWriter.DidNotReceiveWithAnyArgs().WriteExtentBitmap(default!, default!, default!);
        _physicalFileSystem.DidNotReceiveWithAnyArgs().Delete(default!);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-512)]
    public void CreateFile_NonPositivePageSize_ThrowsArgumentOutOfRangeException(int invalidPageSize)
    {
        // Act
        Action act = () => _sut.CreateFile("test.db", FileType.Data, invalidPageSize, 1048576);

        // Assert
        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void CreateFile_PageSizeNotPowerOfTwo_ThrowsArgumentException()
    {
        // Act
        Action act = () => _sut.CreateFile("test.db", FileType.Data, 4097, 1048576);

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void CreateFile_InvalidInitialFileSize_ThrowsArgumentOutOfRangeException()
    {
        // Act
        Action act = () => _sut.CreateFile("test.db", FileType.Data, 4096, -1);

        // Assert
        act.Should().Throw<ArgumentOutOfRangeException>();
    }
}
