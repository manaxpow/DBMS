public abstract class QueryExecutorDecorator : IQueryExecutor
{
    protected readonly IQueryExecutor _innerExecutor;

    public QueryExecutorDecorator(IQueryExecutor innerExecutor) => _innerExecutor = innerExecutor;
    public abstract ResultSet Execute(PhysicalPlan plan);
}
