using System;
using Xunit;
using FluentAssertions;

public class PhysicalOperatorFactoryTests
{
    private readonly PhysicalOperatorFactory _factory = new PhysicalOperatorFactory();

    [Fact]
    public void CreateOperator_GivenLogicalTableScan_ReturnsTableScanOperator()
    {
        // Arrange
        var logicalNode = new LogicalTableScan("Users");

        // Act
        var physicalOperator = _factory.CreateOperator(logicalNode);

        // Assert
        physicalOperator.Should().BeOfType<TableScanOperator>();
        ((TableScanOperator)physicalOperator).TableName.Should().Be("Users");
    }

    [Fact]
    public void CreateOperator_GivenLogicalIndexScan_ReturnsIndexScanOperator()
    {
        var logicalNode = new LogicalIndexScan("Users", "IX_Users_Id");
        var physicalOperator = _factory.CreateOperator(logicalNode);
        physicalOperator.Should().BeOfType<IndexScanOperator>();
    }

    [Fact]
    public void CreateOperator_GivenLogicalHashJoin_ReturnsHashJoinOperator()
    {
        var logicalNode = new LogicalHashJoin();
        var physicalOperator = _factory.CreateOperator(logicalNode);
        physicalOperator.Should().BeOfType<HashJoinOperator>();
    }

    [Fact]
    public void CreateOperator_GivenLogicalNestedLoopJoin_ReturnsNestedLoopJoinOperator()
    {
        var logicalNode = new LogicalNestedLoopJoin();
        var physicalOperator = _factory.CreateOperator(logicalNode);
        physicalOperator.Should().BeOfType<NestedLoopJoinOperator>();
    }

    [Fact]
    public void CreateOperator_GivenLogicalSort_ReturnsSortOperator()
    {
        var logicalNode = new LogicalSort();
        var physicalOperator = _factory.CreateOperator(logicalNode);
        physicalOperator.Should().BeOfType<SortOperator>();
    }

    [Fact]
    public void CreateOperator_GivenUnsupportedNode_ThrowsNotSupportedException()
    {
        var logicalNode = new UnsupportedLogicalNode();

        Action act = () => _factory.CreateOperator(logicalNode);

        act.Should().Throw<NotSupportedException>()
           .WithMessage("Unsupported logical node: UnsupportedLogicalNode");
    }
}
