public class ProfilingDecorator : QueryExecutorDecorator
{
    public ProfilingDecorator(IQueryExecutor innerExecutor) : base(innerExecutor) { }
    public override ResultSet Execute(PhysicalPlan plan)
    {
        throw new NotImplementedException();
    }
}
