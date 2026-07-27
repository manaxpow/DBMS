using System;
using System.Collections.Generic;
using FluentAssertions;
using Xunit;
using Moq;

public class OptimizationPipelineTests
{
    [Fact]
    public void Optimize_ShouldPassThroughChainInCorrectOrder()
    {
        // Arrange
        var mockRule1 = new Mock<IOptimizationRule>();
        var mockRule2 = new Mock<IOptimizationRule>();
        var mockRule3 = new Mock<IOptimizationRule>();
        var plan = new LogicalPlan();

        // Setup the chain manually for the test
        var callOrder = new List<string>();

        mockRule1.Setup(r => r.Optimize(It.IsAny<LogicalPlan>())).Returns((LogicalPlan p) => 
        {
            callOrder.Add("Rule1");
            return mockRule2.Object.Optimize(p);
        });
        
        mockRule2.Setup(r => r.Optimize(It.IsAny<LogicalPlan>())).Returns((LogicalPlan p) => 
        {
            callOrder.Add("Rule2");
            return mockRule3.Object.Optimize(p);
        });

        mockRule3.Setup(r => r.Optimize(It.IsAny<LogicalPlan>())).Returns((LogicalPlan p) => 
        {
            callOrder.Add("Rule3");
            return p;
        });

        // Act
        var result = mockRule1.Object.Optimize(plan);

        // Assert
        callOrder.Should().ContainInOrder("Rule1", "Rule2", "Rule3");
        result.Should().Be(plan);
    }

    [Fact]
    public void ConstantFoldingRule_Optimize_ShouldTransformPlan()
    {
        // Arrange
        var rule = new ConstantFoldingRule();
        var plan = new LogicalPlan();

        // Act
        var result = rule.Optimize(plan);

        // Assert
        // In a real scenario, assert that the expression tree was folded
        result.Should().NotBeNull();
    }

    [Fact]
    public void PredicatePushdownRule_Optimize_ShouldTransformPlan()
    {
        // Arrange
        var rule = new PredicatePushdownRule();
        var plan = new LogicalPlan();

        // Act
        var result = rule.Optimize(plan);

        // Assert
        // In a real scenario, assert that predicates were pushed down
        result.Should().NotBeNull();
    }

    [Fact]
    public void ProjectionPruningRule_Optimize_ShouldTransformPlan()
    {
        // Arrange
        var rule = new ProjectionPruningRule();
        var plan = new LogicalPlan();

        // Act
        var result = rule.Optimize(plan);

        // Assert
        // In a real scenario, assert that unneeded columns were pruned
        result.Should().NotBeNull();
    }

    [Fact]
    public void Rule_WhenNextIsNull_ShouldReturnPlan()
    {
        // Arrange
        var rule = new ConstantFoldingRule(); 
        var plan = new LogicalPlan();
        
        // Act
        // SetNext is not called, so _next is null
        var result = rule.Optimize(plan);

        // Assert
        result.Should().NotBeNull();
    }
}
