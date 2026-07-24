using System;

public class RuleBasedOptimizationStrategy : IOptimizationStrategy
{
    public PhysicalPlan Optimize(LogicalPlan plan)
    {
        plan = ApplyPredicatePushdown(plan);
        plan = ApplyProjectionPruning(plan);
        plan = ApplyConstantFolding(plan);
        return new PhysicalPlan();
    }

    public virtual LogicalPlan ApplyPredicatePushdown(LogicalPlan plan)
    {
        return plan;
    }

    public virtual LogicalPlan ApplyProjectionPruning(LogicalPlan plan)
    {
        return plan;
    }

    public virtual LogicalPlan ApplyConstantFolding(LogicalPlan plan)
    {
        return plan;
    }
}
