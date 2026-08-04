using System;
using FluentAssertions;
using Xunit;

public class PageTests
{

    [Trait("Category", "Important")]
    [Fact]
    public void InsertRecord_WhenSpaceIsAvailable_ShouldInsertRecord()
    {
        // Arrange
        var page = new Page(new PageId(1), new byte[4096]);
        var record = new Record(1, new byte[100]);

        // Act
        var result = page.InsertRecord(record);

        // Assert
        result.Should().NotBeNull();
    }

    [Trait("Category", "Important")]
    [Fact]
    public void InsertRecord_WhenSpaceIsInsufficient_ShouldFail()
    {
        // Arrange
        var page = new Page(new PageId(1), new byte[4096]);
        var record = new Record(1, new byte[4097]);

        // Act
        var result = page.InsertRecord(record);

        // Assert
        result.Should().BeNull();
    }

    [Trait("Category", "Important")]
    [Fact]
    public void DeleteRecord_WhenRecordExists_ShouldUpdateSlotDirectory()
    {
        // Arrange
        var page = new Page(new PageId(1), new byte[4096]);
        var record = new Record(1, new byte[100]);
        var result = page.InsertRecord(record);

        // Act
        page.DeleteRecord(1);

        // Assert
        page.Read().Should().BeEquivalentTo(new byte[4096]);
    }


    [Fact]
    public void InsertRecord_WhenRecordExceedsPageCapacity_ShouldFail()
    {
        throw new NotImplementedException();
    }

    [Trait("Category", "Important")]
    [Fact]
    public void GetRecord_WhenSlotExists_ShouldReturnRecord()
    {
        // Arrange
        var page = new Page(new PageId(1), new byte[4096]);
        var record = new Record(1, new byte[100]);
        var result = page.InsertRecord(record);

        // Act
        var getResult = page.GetRecord(1);

        // Assert
        getResult.Should().BeEquivalentTo(record);
    }

    [Fact]
    public void GetRecord_WhenSlotDoesNotExist_ShouldThrow()
    {
        throw new NotImplementedException();
    }

    [Trait("Category", "Important")]
    [Fact]
    public void UpdateRecord_WhenSpaceIsSufficient_ShouldModifyRecord()
    {
        // Arrange
        var page = new Page(new PageId(1), new byte[4096]);
        var record = new Record(1, new byte[100]);
        var result = page.InsertRecord(record);

        var updatedRecord = new Record(1, new byte[200]);
        // Act

        page.UpdateRecord(record);
        // Assert

        page.Read().Should().BeEquivalentTo(updatedRecord);
    }

    [Trait("Category", "Important")]
    [Fact]
    public void UpdateRecord_WhenSpaceIsInsufficient_ShouldPreserveOriginalRecord()
    {
        // Arrange
        var page = new Page(new PageId(1), new byte[4096]);
        var record = new Record(1, new byte[100]);
        page.InsertRecord(record);

        var updatedRecord = new Record(1, new byte[4097]);
        // Act

        page.UpdateRecord(updatedRecord);
        // Assert

        page.Read().Should().BeEquivalentTo(record);
    }

    [Fact]
    public void DeleteRecord_WhenRecordDoesNotExist_ShouldReturnFalse()
    {
        throw new NotImplementedException();
    }

    [Fact]
    public void DeleteRecord_WhenRecordIsAlreadyDeleted_ShouldReturnFalse()
    {
        throw new NotImplementedException();
    }

    [Trait("Category", "Important")]
    [Fact]
    public void Compact_WhenDeletedRecordsExist_ShouldReclaimSpace()
    {
        // Arrange
        var page = new Page(new PageId(1), new byte[4096]);
        var record = new Record(1, new byte[100]);
        page.InsertRecord(record);

        // Act
        page.Compact();

        // Assert
        page.Read().Should().BeEquivalentTo(new byte[4096]);
    }

    [Trait("Category", "Important")]
    [Fact]
    public void InsertRecord_WhenDeletedSlotExists_ShouldReuseSlot()
    {
        // Arrange
        var page = new Page(new PageId(1), new byte[4096]);
        var record = new Record(1, new byte[100]);
        page.InsertRecord(record);
        page.DeleteRecord(1);

        // Act
        var result = page.InsertRecord(record);

        // Assert
        result.Should().NotBeNull();
    }
}
