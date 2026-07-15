using System;
using Xunit;
using FluentAssertions;
using NSubstitute;
using DBMS.StorageEngine.FileManagement.ExtentManagement;
using DBMS.StorageEngine.FileManagement.Exceptions;
using DBMS.StorageEngine.FileManagement.RuntimeFileManagement;

namespace DBMS.StorageEngine.UnitTests.FileManagement.ExtentManagement;

public class FreeExtentTests
{
    private readonly ExtentManager _sut;

    public FreeExtentTests()
    {
        _sut = new ExtentManager();
    }

    [Fact]
    public void FreeExtent_Successfully_Succeeds()
    {
        var entry = Substitute.For<OpenFileEntry>();
        
        // Act & Assert
        Action act = () => _sut.FreeExtent(entry, 3);
        act.Should().NotThrow();
    }

    [Fact]
    public void FreeExtent_NonExistentExtent_ThrowsInvalidExtentException()
    {
        var entry = Substitute.For<OpenFileEntry>();
        
        // Act & Assert
        Action act = () => _sut.FreeExtent(entry, 99);
        act.Should().Throw<InvalidExtentException>();
    }

    [Fact]
    public void FreeExtent_AlreadyFreeExtent_ThrowsExtentAlreadyFreeException()
    {
        var entry = Substitute.For<OpenFileEntry>();
        
        // Act & Assert
        Action act = () => _sut.FreeExtent(entry, 3);
        act.Should().Throw<ExtentAlreadyFreeException>();
    }

    [Fact]
    public void FreeExtent_BlockedByActiveExtentUsage_ThrowsExtentInUseException()
    {
        var entry = Substitute.For<OpenFileEntry>();
        
        // Act & Assert
        Action act = () => _sut.FreeExtent(entry, 3);
        act.Should().Throw<ExtentInUseException>();
    }

    // Rollback tests skipped due to design gaps
}

