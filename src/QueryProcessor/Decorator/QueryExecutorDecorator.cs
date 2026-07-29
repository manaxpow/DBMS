public abstract class QueryExecutorDecorator : IQueryExecutor
{
    private readonly IQueryExecutor innerExecutor;

    public QueryExecutorDecorator(IQueryExecutor innerExecutor) => this.innerExecutor = innerExecutor;

    protected IQueryExecutor InnerExecutor => this.innerExecutor;

    public abstract ResultSet Execute(PhysicalPlan plan);
}
