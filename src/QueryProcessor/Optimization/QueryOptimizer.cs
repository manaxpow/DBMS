using System;

public class QueryOptimizer : IQueryOptimizer
{
    private object _rules; 

    public LogicalPlan Optimize(LogicalPlan plan)
    {
        return default;
    }

    public void ApplyRules()
    {
    }
}
