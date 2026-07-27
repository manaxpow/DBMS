public class OptimizationRuleBase : IOptimizationRule
{
    private IOptimizationRule _next;

    public void SetNext(IOptimizationRule rule)
    {
        _next = rule;
    }

    public virtual LogicalPlan Optimize(LogicalPlan plan)
    {
        throw new NotImplementedException();
    }
}
