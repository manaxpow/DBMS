using System;
using Xunit;
using FluentAssertions;
using NSubstitute;

public class QueryOptimizerTests
{
    // --------------------------------------------------------------------
    // QueryOptimizer Tests (Context)
    // --------------------------------------------------------------------

    [Trait("Category", "Important")]
    [Fact]
    public void QueryOptimizer_Optimize_WhenStrategyIsNull_ShouldThrow()
    {
        // Arrange
        var optimizer = new QueryOptimizer(null);
        var plan = new LogicalPlan();

        // Act
        Action act = () => optimizer.Optimize(plan);

        // Assert
        act.Should().Throw<InvalidOperationException>()
           .WithMessage("Optimization strategy not set.");
    }

    [Trait("Category", "Important")]
    [Fact]
    public void QueryOptimizer_Optimize_ShouldDelegateToStrategy()
    {
        // Arrange
        var mockStrategy = Substitute.For<IOptimizationStrategy>();
        var expectedPhysicalPlan = new PhysicalPlan { Cost = 42, OperatorType = PhysicalOperatorType.TableScan };

        mockStrategy.Optimize(Arg.Any<LogicalPlan>()).Returns(expectedPhysicalPlan);

        var optimizer = new QueryOptimizer(mockStrategy);
        var logicalPlan = new LogicalPlan();

        // Act
        var result = optimizer.Optimize(logicalPlan);

        // Assert
        result.Should().Be(expectedPhysicalPlan);
        mockStrategy.Received(1).Optimize(logicalPlan);
    }
}


