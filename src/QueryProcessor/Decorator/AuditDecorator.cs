public class AuditDecorator : QueryExecutorDecorator
{
    public AuditDecorator(IQueryExecutor innerExecutor) : base(innerExecutor) { }
    public override ResultSet Execute(PhysicalPlan plan)
    {
        throw new NotImplementedException();
    }
}
