public interface IOptimizationRule
{
    void SetNext(IOptimizationRule rule);

    LogicalPlan Optimize(LogicalPlan plan);
}
