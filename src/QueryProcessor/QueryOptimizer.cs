using System;

public class QueryOptimizer
{
    private IOptimizationStrategy _strategy;

    public QueryOptimizer(IOptimizationStrategy strategy = null)
    {
        _strategy = strategy;
    }

    public void SetStrategy(IOptimizationStrategy strategy)
    {
        _strategy = strategy;
    }

    public PhysicalPlan Optimize(LogicalPlan plan)
    {
        if (_strategy == null)
        {
            throw new InvalidOperationException("Optimization strategy not set.");
        }
        return _strategy.Optimize(plan);
    }
}
