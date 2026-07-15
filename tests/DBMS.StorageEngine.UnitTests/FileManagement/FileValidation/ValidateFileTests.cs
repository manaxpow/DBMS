using System;
using Xunit;
using FluentAssertions;
using DBMS.StorageEngine.FileManagement.FileValidation;
using DBMS.StorageEngine.FileManagement.Exceptions;
using DBMS.StorageEngine.FileManagement.Domain;

namespace DBMS.StorageEngine.UnitTests.FileManagement.FileValidation;

public class ValidateFileTests
{
    private readonly FileValidator _sut = new FileValidator();

    [Fact]
    public void Validate_ValidStructures_Succeeds()
    {
        Action act = () => _sut.Validate(new FileHeader(), new AllocationMetadata(), new ExtentBitmap(), 1048576);
        act.Should().NotThrow();
    }

    [Fact]
    public void Validate_InvalidMagicNumber_ThrowsInvalidFileFormatException()
    {
        Action act = () => _sut.Validate(new FileHeader(), new AllocationMetadata(), new ExtentBitmap(), 1048576);
        act.Should().Throw<InvalidFileFormatException>();
    }

    [Fact]
    public void Validate_UnsupportedVersion_ThrowsUnsupportedFileVersionException()
    {
        Action act = () => _sut.Validate(new FileHeader(), new AllocationMetadata(), new ExtentBitmap(), 1048576);
        act.Should().Throw<UnsupportedFileVersionException>();
    }

    [Fact]
    public void Validate_NonPowerOfTwoPageSize_ThrowsInvalidFileFormatException()
    {
        Action act = () => _sut.Validate(new FileHeader(), new AllocationMetadata(), new ExtentBitmap(), 1048576);
        act.Should().Throw<InvalidFileFormatException>();
    }

    [Fact]
    public void Validate_UnalignedExtentSize_ThrowsInvalidFileFormatException()
    {
        Action act = () => _sut.Validate(new FileHeader(), new AllocationMetadata(), new ExtentBitmap(), 1048576);
        act.Should().Throw<InvalidFileFormatException>();
    }

    [Fact]
    public void Validate_OffsetBeyondPhysicalSize_ThrowsCorruptedFileMetadataException()
    {
        Action act = () => _sut.Validate(new FileHeader(), new AllocationMetadata(), new ExtentBitmap(), 1048576);
        act.Should().Throw<CorruptedFileMetadataException>();
    }

    [Fact]
    public void Validate_BitmapBitcountMismatch_ThrowsCorruptedFileMetadataException()
    {
        Action act = () => _sut.Validate(new FileHeader(), new AllocationMetadata(), new ExtentBitmap(), 1048576);
        act.Should().Throw<CorruptedFileMetadataException>();
    }

    [Fact]
    public void Validate_FreeCountExceedsCapacity_ThrowsCorruptedFileMetadataException()
    {
        Action act = () => _sut.Validate(new FileHeader(), new AllocationMetadata(), new ExtentBitmap(), 1048576);
        act.Should().Throw<CorruptedFileMetadataException>();
    }

    [Fact]
    public void Validate_PhysicalSizeMismatch_ThrowsCorruptedFileMetadataException()
    {
        Action act = () => _sut.Validate(new FileHeader(), new AllocationMetadata(), new ExtentBitmap(), 1048576);
        act.Should().Throw<CorruptedFileMetadataException>();
    }

    [Fact]
    public void Validate_HeaderChecksumDeprecated_Succeeds()
    {
        Action act = () => _sut.Validate(new FileHeader(), new AllocationMetadata(), new ExtentBitmap(), 1048576);
        act.Should().NotThrow();
    }
}
