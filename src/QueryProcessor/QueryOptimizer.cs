using System;

public class QueryOptimizer
{
    private IOptimizationStrategy strategy;

    public QueryOptimizer(IOptimizationStrategy strategy)
    {
        this.strategy = strategy;
    }

    public void SetStrategy(IOptimizationStrategy strategy) => throw new NotImplementedException();

    public PhysicalPlan Optimize(LogicalPlan plan) => throw new NotImplementedException();
}
