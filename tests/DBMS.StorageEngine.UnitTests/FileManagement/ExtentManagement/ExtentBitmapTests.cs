using System;
using Xunit;
using FluentAssertions;
using DBMS.StorageEngine.FileManagement.Domain;

namespace DBMS.StorageEngine.UnitTests.FileManagement.ExtentManagement;

public class ExtentBitmapTests
{
    [Fact]
    public void Create_NewBitmap_HasAllBitsFree()
    {
        Action act = () => ExtentBitmap.Create(16);
        act.Should().NotThrow();
    }

    [Fact]
    public void MarkUsed_ValidIndex_TransitionsBit()
    {
        Action act = () => new ExtentBitmap().MarkUsed(2);
        act.Should().NotThrow();
    }

    [Fact]
    public void MarkUsed_Duplicate_ThrowsInvalidOperationException()
    {
        Action act = () => new ExtentBitmap().MarkUsed(2);
        act.Should().Throw<InvalidOperationException>();
    }
}

