using System.Collections.Generic;
using FluentAssertions;
using Xunit;

public class TableScanOperatorTests
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
    public void Open_ShouldInitializeEnumerator()
    {
        // Arrange
        var rows = CreateMockRows();
        var sut = new TableScanOperator("TestTable", rows);

        // Act
        sut.Open();

        // Assert
        var hasNext = sut.Next();
        hasNext.Should().BeTrue();
        sut.GetCurrent().Should().Be(rows[0]);
    }

    [Fact]
    public void Next_WhenRowsExist_ShouldReturnTrue()
    {
        // Arrange
        var rows = CreateMockRows();
        var sut = new TableScanOperator("TestTable", rows);
        sut.Open();

        // Act & Assert
        sut.Next().Should().BeTrue();
        sut.Next().Should().BeTrue();
        sut.Next().Should().BeTrue();
    }

    [Fact]
    public void Next_WhenNoMoreRows_ShouldReturnFalse()
    {
        // Arrange
        var rows = CreateMockRows();
        var sut = new TableScanOperator("TestTable", rows);
        sut.Open();

        sut.Next();
        sut.Next();
        sut.Next();

        // Act
        var hasNext = sut.Next();

        // Assert
        hasNext.Should().BeFalse();
    }

    [Fact]
    public void GetCurrent_ShouldReturnCorrectRow()
    {
        // Arrange
        var rows = CreateMockRows();
        var sut = new TableScanOperator("TestTable", rows);
        sut.Open();

        // Act
        sut.Next();
        var current1 = sut.GetCurrent();

        sut.Next();
        var current2 = sut.GetCurrent();

        // Assert
        current1.Should().Be(rows[0]);
        current2.Should().Be(rows[1]);
    }
}


