using System;
using Xunit;
using FluentAssertions;

public class PhysicalOperatorFactoryTests
{
    private readonly PhysicalOperatorFactory _factory = new PhysicalOperatorFactory();

    [Fact]
    public void CreateOperator_GivenLogicalTableScan_ReturnsTableScanOperator()
    {
        throw new System.NotImplementedException();
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
        throw new System.NotImplementedException();
        var logicalNode = new LogicalIndexScan("Users", "IX_Users_Id");
        var physicalOperator = _factory.CreateOperator(logicalNode);
        physicalOperator.Should().BeOfType<IndexScanOperator>();
    }

    [Fact]
    public void CreateOperator_GivenLogicalHashJoin_ReturnsHashJoinOperator()
    {
        throw new System.NotImplementedException();
        var logicalNode = new LogicalHashJoin();
        var physicalOperator = _factory.CreateOperator(logicalNode);
        physicalOperator.Should().BeOfType<HashJoinOperator>();
    }

    [Fact]
    public void CreateOperator_GivenLogicalNestedLoopJoin_ReturnsNestedLoopJoinOperator()
    {
        throw new System.NotImplementedException();
        var logicalNode = new LogicalNestedLoopJoin();
        var physicalOperator = _factory.CreateOperator(logicalNode);
        physicalOperator.Should().BeOfType<NestedLoopJoinOperator>();
    }

    [Fact]
    public void CreateOperator_GivenLogicalSort_ReturnsSortOperator()
    {
        throw new System.NotImplementedException();
        var logicalNode = new LogicalSort();
        var physicalOperator = _factory.CreateOperator(logicalNode);
        physicalOperator.Should().BeOfType<SortOperator>();
    }

    [Fact]
    public void CreateOperator_GivenUnsupportedNode_ThrowsNotSupportedException()
    {
        throw new System.NotImplementedException();
        var logicalNode = new UnsupportedLogicalNode();

        Action act = () => _factory.CreateOperator(logicalNode);

        act.Should().Throw<NotSupportedException>()
           .WithMessage("Unsupported logical node: UnsupportedLogicalNode");
    }
}

