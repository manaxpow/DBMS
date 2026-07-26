using System;

public class QueryOptimizer
{
    private IOptimizationStrategy _strategy;

    public QueryOptimizer(IOptimizationStrategy strategy = null)
    {
        _strategy = strategy;
    }

    public void SetStrategy(IOptimizationStrategy strategy) => throw new NotImplementedException();
    public PhysicalPlan Optimize(LogicalPlan plan) => throw new NotImplementedException();
}
