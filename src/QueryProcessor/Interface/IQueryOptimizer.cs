using System;

public interface IQueryOptimizer
{
    LogicalPlan Optimize(LogicalPlan plan);
}
