using System;
using System.Collections.Generic;
using FluentAssertions;
using Xunit;
using NSubstitute;

public class FilterOperatorTests
{
    private List<Row> CreateMockRows()
    {
        return new List<Row>
        {
            new Row(null!, new List<object> { 1, "Alice" }),
            new Row(null!, new List<object> { 2, "Bob" }),
            new Row(null!, new List<object> { 3, "Charlie" })
        };
    }

    [Fact]
    public void Next_WhenRowMatchesPredicate_ShouldReturnTrue()
    {
        // Arrange
        var rows = CreateMockRows();
        var child = new TableScanOperator("Test", rows);
        var sut = new FilterOperator(child, r => (int)r.Values[0] > 1); // Bob and Charlie

        sut.Open();

        // Act
        var hasNext = sut.Next();

        // Assert
        hasNext.Should().BeTrue();
        sut.GetCurrent().Should().Be(rows[1]); // Bob
    }

    [Fact]
    public void Next_WhenNoRowMatches_ShouldReturnFalse()
    {
        // Arrange
        var rows = CreateMockRows();
        var child = new TableScanOperator("Test", rows);
        var sut = new FilterOperator(child, r => (int)r.Values[0] > 10);

        sut.Open();

        // Act
        var hasNext = sut.Next();

        // Assert
        hasNext.Should().BeFalse();
    }

    [Fact]
    public void GetCurrent_ShouldReturnMatchedRow()
    {
        // Arrange
        var rows = CreateMockRows();
        var child = new TableScanOperator("Test", rows);
        var sut = new FilterOperator(child, r => (string)r.Values[1] == "Charlie");

        sut.Open();

        // Act
        sut.Next();
        var current = sut.GetCurrent();

        // Assert
        current.Should().Be(rows[2]);
    }

    [Fact]
    public void Close_ShouldCloseChildOperator()
    {
        // Arrange
        var child = Substitute.For<PhysicalOperator>();
        var sut = new FilterOperator(child, r => true);

        // Act
        sut.Close();

        // Assert
        child.Received(1).Close();
    }
}


