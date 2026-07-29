using System;

public class RuleBasedOptimizationStrategy : IOptimizationStrategy
{
    public PhysicalPlan Optimize(LogicalPlan plan) => throw new NotImplementedException();

    public virtual LogicalPlan ApplyPredicatePushdown(LogicalPlan plan) => throw new NotImplementedException();

    public virtual LogicalPlan ApplyProjectionPruning(LogicalPlan plan) => throw new NotImplementedException();

    public virtual LogicalPlan ApplyConstantFolding(LogicalPlan plan) => throw new NotImplementedException();
}
