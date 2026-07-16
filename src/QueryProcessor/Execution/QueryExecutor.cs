using System;

public class QueryExecutor : IQueryExecutor
{
    private object _factory; 
    public QueryResult Execute(PhysicalPlan plan, ExecutionContext ctx)
    {
        return default;
    }

    public void RunPipeline()
    {
    }
}
