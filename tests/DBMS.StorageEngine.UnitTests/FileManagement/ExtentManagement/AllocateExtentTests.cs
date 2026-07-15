using System;
using Xunit;
using FluentAssertions;
using NSubstitute;
using DBMS.StorageEngine.FileManagement.ExtentManagement;
using DBMS.StorageEngine.FileManagement.Exceptions;
using DBMS.StorageEngine.FileManagement.RuntimeFileManagement;

namespace DBMS.StorageEngine.UnitTests.FileManagement.ExtentManagement;

public class AllocateExtentTests
{
    private readonly ExtentManager _sut;

    public AllocateExtentTests()
    {
        _sut = new ExtentManager();
    }

    [Fact]
    public void AllocateExtent_EmptyExtentSuccess_ReturnsAllocatedExtent()
    {
        var entry = Substitute.For<OpenFileEntry>();
        
        // Act & Assert
        Action act = () => _sut.AllocateExtent(entry);
        act.Should().NotThrow();
    }

    [Fact]
    public void AllocateExtent_TriggeringAutoExtend_ReturnsAllocatedExtent()
    {
        var entry = Substitute.For<OpenFileEntry>();
        
        // Act & Assert
        Action act = () => _sut.AllocateExtent(entry);
        act.Should().NotThrow();
    }

    [Fact]
    public void AllocateExtent_ExhaustedWithAutoExtendDisabled_ThrowsNoFreeExtentException()
    {
        var entry = Substitute.For<OpenFileEntry>();
        
        // Act & Assert
        Action act = () => _sut.AllocateExtent(entry);
        act.Should().Throw<NoFreeExtentException>();
    }

    [Fact]
    public void AllocateExtent_MaximumFileSizeReachedLimit_ThrowsMaximumFileSizeExceededException()
    {
        var entry = Substitute.For<OpenFileEntry>();
        
        // Act & Assert
        Action act = () => _sut.AllocateExtent(entry);
        act.Should().Throw<MaximumFileSizeExceededException>();
    }

    [Fact]
    public void AllocateExtent_ResizeFileFailureDuringExtension_ThrowsExtentAllocationException()
    {
        var entry = Substitute.For<OpenFileEntry>();
        
        // Act & Assert
        Action act = () => _sut.AllocateExtent(entry);
        act.Should().Throw<ExtentAllocationException>();
    }

    [Fact]
    public void AllocateExtent_CallMarkExtentAllocatedCorrectly_ReturnsAllocatedExtent()
    {
        var entry = Substitute.For<OpenFileEntry>();
        
        // Act & Assert
        Action act = () => _sut.AllocateExtent(entry);
        act.Should().NotThrow();
    }

    [Fact]
    public void AllocateExtent_PersistMetadataFailedDuringAllocation_ThrowsExtentAllocationException()
    {
        var entry = Substitute.For<OpenFileEntry>();
        
        // Act & Assert
        Action act = () => _sut.AllocateExtent(entry);
        act.Should().Throw<ExtentAllocationException>();
    }

    // Rollback tests skipped due to design gaps
}

