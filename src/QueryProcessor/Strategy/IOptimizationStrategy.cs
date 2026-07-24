using System;

public interface IOptimizationStrategy
{
    PhysicalPlan Optimize(LogicalPlan plan);
}
