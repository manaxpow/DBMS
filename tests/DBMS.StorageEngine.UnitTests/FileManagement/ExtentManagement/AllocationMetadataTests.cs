using System;
using Xunit;
using FluentAssertions;
using DBMS.StorageEngine.FileManagement.Domain;
using DBMS.StorageEngine.FileManagement.Exceptions;

namespace DBMS.StorageEngine.UnitTests.FileManagement.ExtentManagement;

public class AllocationMetadataTests
{
    private AllocationMetadata CreateSut(int extentSize, int initialExtents)
    {
        return AllocationMetadata.Create(extentSize, initialExtents);
    }

    [Fact]
    public void Create_InitializesCorrectly()
    {
        var sut = CreateSut(4096, 10);
        sut.TotalExtentCount.Should().Be(10);
        sut.FreeExtentCount.Should().Be(10);
        sut.ExtentSize.Should().Be(4096);
        sut.ExtentBitmap.Should().NotBeNull();
        
        var firstFree = sut.ExtentBitmap.FindFirstFree();
        firstFree.Should().Be(0);
    }

    [Fact]
    public void MarkExtentAllocated_WhenExtentIsFree_ShouldUpdateCountsAndBitmap()
    {
        var sut = CreateSut(4096, 10);
        
        sut.MarkExtentAllocated(0);
        
        sut.FreeExtentCount.Should().Be(9);
        sut.TotalExtentCount.Should().Be(10);
        sut.ExtentBitmap.IsAllocated(0).Should().BeTrue();
    }

    [Fact]
    public void MarkExtentAllocated_WhenExtentIsAlreadyAllocated_ShouldThrowInvalidOperationException()
    {
        var sut = CreateSut(4096, 10);
        sut.MarkExtentAllocated(0);
        
        Action act = () => sut.MarkExtentAllocated(0);
        
        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void MarkExtentAllocated_WhenExtentIsOutOfBounds_ShouldThrowArgumentOutOfRangeException()
    {
        var sut = CreateSut(4096, 10);
        Action act = () => sut.MarkExtentAllocated(10);
        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void MarkExtentFree_WhenExtentIsAllocated_ShouldUpdateCountsAndBitmap()
    {
        var sut = CreateSut(4096, 10);
        sut.MarkExtentAllocated(0);
        
        sut.MarkExtentFree(0);
        
        sut.FreeExtentCount.Should().Be(10);
        sut.TotalExtentCount.Should().Be(10);
        sut.ExtentBitmap.IsAllocated(0).Should().BeFalse();
    }

    [Fact]
    public void MarkExtentFree_WhenExtentIsAlreadyFree_ShouldThrowInvalidOperationException()
    {
        var sut = CreateSut(4096, 10);
        Action act = () => sut.MarkExtentFree(0);
        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void AddExtents_ShouldIncreaseTotalAndFreeCountsAndUpdateBitmap()
    {
        var sut = CreateSut(4096, 10);
        sut.MarkExtentAllocated(0); // 9 free
        
        sut.AddExtents(5);
        
        sut.TotalExtentCount.Should().Be(15);
        sut.FreeExtentCount.Should().Be(14);
        
        // Ensure new extents are free
        sut.ExtentBitmap.IsAllocated(10).Should().BeFalse();
        sut.ExtentBitmap.IsAllocated(14).Should().BeFalse();
    }

    [Fact]
    public void CanTruncateTo_WhenExtentsBeyondTargetAreFree_ShouldReturnTrue()
    {
        var sut = CreateSut(4096, 10);
        sut.MarkExtentAllocated(0);
        sut.MarkExtentAllocated(4);
        
        // Target is 5 (so indexes 0 to 4 are kept, 5 to 9 are truncated)
        // Indexes 5 to 9 are free, so it should be allowed
        sut.CanTruncateTo(5).Should().BeTrue();
    }

    [Fact]
    public void CanTruncateTo_WhenExtentsBeyondTargetAreAllocated_ShouldReturnFalse()
    {
        var sut = CreateSut(4096, 10);
        sut.MarkExtentAllocated(8); // Allocated an extent that would be truncated
        
        sut.CanTruncateTo(5).Should().BeFalse();
    }

    [Fact]
    public void TruncateTo_WhenValid_ShouldUpdateCountsAndBitmap()
    {
        var sut = CreateSut(4096, 10);
        sut.MarkExtentAllocated(0); // 1 allocated, 9 free
        
        sut.TruncateTo(5);
        
        sut.TotalExtentCount.Should().Be(5);
        sut.FreeExtentCount.Should().Be(4); // Only 1 allocated, so 4 free
        
        Action act = () => sut.ExtentBitmap.IsAllocated(5); // Should be out of bounds
        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void TruncateTo_WhenInvalid_ShouldThrowInvalidOperationException()
    {
        var sut = CreateSut(4096, 10);
        sut.MarkExtentAllocated(8);
        
        Action act = () => sut.TruncateTo(5);
        
        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void CalculateFileOffset_ShouldReturnCorrectOffset()
    {
        var sut = CreateSut(4096, 10);
        var header = FileHeader.Create(new FileHeaderConfiguration(new FileId(), FileType.Data, 1024, 4096, 1, 1024, 1024));
        
        // Assuming DataRegionOffset is 1024 and ExtentSize is 4096
        // Extent 0 -> 1024
        // Extent 1 -> 1024 + 4096 = 5120
        var offset = sut.CalculateFileOffset(1, header);
        
        offset.Should().Be(1024 + 4096);
    }
}
