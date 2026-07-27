public class QueryExecutionLoggerDecorator : QueryExecutorDecorator
{
    public QueryExecutionLoggerDecorator(IQueryExecutor innerExecutor) : base(innerExecutor) { }

    public override ResultSet Execute(PhysicalPlan plan)
    {
        throw new NotImplementedException();
    }
}
