using FluentAssertions;

public class CostBasedOptimizationStrategyTests
{
    [Trait("Category", "Important")]
    [Fact]
    public void SelectBestPlan_WhenMultiplePlansExist_ShouldChooseLowestCostPlan()
    {
        // Arrange
        var strategy = new CostBasedOptimizationStrategy();
        var plan1 = new PhysicalPlan { Cost = 100, OperatorType = PhysicalOperatorType.TableScan };
        var plan2 = new PhysicalPlan { Cost = 10, OperatorType = PhysicalOperatorType.IndexScan };
        var plan3 = new PhysicalPlan { Cost = 50, OperatorType = PhysicalOperatorType.NestedLoopJoin };

        var candidates = new List<PhysicalPlan> { plan1, plan2, plan3 };

        // Act
        var result = strategy.SelectBestPlan(candidates);

        // Assert
        result.Should().Be(plan2);
        result.Cost.Should().Be(10);
        result.OperatorType.Should().Be(PhysicalOperatorType.IndexScan);
    }

    [Trait("Category", "Important")]
    [Fact]
    public void SelectBestPlan_WhenIndexScanIsCheaper_ShouldChooseIndexScan()
    {
        // Arrange
        var strategy = new CostBasedOptimizationStrategy();
        var tableScanPlan = new PhysicalPlan { Cost = 500, OperatorType = PhysicalOperatorType.TableScan };
        var indexScanPlan = new PhysicalPlan { Cost = 50, OperatorType = PhysicalOperatorType.IndexScan };

        var candidates = new List<PhysicalPlan> { tableScanPlan, indexScanPlan };

        // Act
        var result = strategy.SelectBestPlan(candidates);

        // Assert
        result.Should().Be(indexScanPlan);
        result.Cost.Should().Be(50);
        result.OperatorType.Should().Be(PhysicalOperatorType.IndexScan);
    }

    [Trait("Category", "Important")]
    [Fact]
    public void SelectBestPlan_WhenIndexIsUnavailable_ShouldChooseTableScan()
    {
        // Arrange
        var strategy = new CostBasedOptimizationStrategy();
        var tableScanPlan = new PhysicalPlan { Cost = 500, OperatorType = PhysicalOperatorType.TableScan };

        var candidates = new List<PhysicalPlan> { tableScanPlan };

        // Act
        var result = strategy.SelectBestPlan(candidates);

        // Assert
        result.Should().Be(tableScanPlan);
        result.OperatorType.Should().Be(PhysicalOperatorType.TableScan);
    }
}
