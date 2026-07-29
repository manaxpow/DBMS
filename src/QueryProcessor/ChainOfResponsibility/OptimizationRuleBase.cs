public class OptimizationRuleBase : IOptimizationRule
{
    private IOptimizationRule next = null!;

    public void SetNext(IOptimizationRule rule)
    {
        this.next = rule;
    }

    public virtual LogicalPlan Optimize(LogicalPlan plan)
    {
        throw new NotImplementedException();
    }
}
