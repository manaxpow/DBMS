public abstract class QueryExecutorDecorator : IQueryExecutor
{
    private readonly IQueryExecutor _innerExecutor;

    public QueryExecutorDecorator(IQueryExecutor innerExecutor) => this._innerExecutor = innerExecutor;

    protected IQueryExecutor InnerExecutor => this._innerExecutor;

    public abstract ResultSet Execute(PhysicalPlan plan);
}
