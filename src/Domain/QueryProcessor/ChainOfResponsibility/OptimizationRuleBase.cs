public class OptimizationRuleBase : IOptimizationRule
{
    private IOptimizationRule _next = null!;

    public void SetNext(IOptimizationRule rule)
    {
        this._next = rule;
    }

    public virtual LogicalPlan Optimize(LogicalPlan plan)
    {
        throw new NotImplementedException();
    }
}
