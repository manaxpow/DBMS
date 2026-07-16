using System;

public interface IQueryExecutor
{
    QueryResult Execute(PhysicalPlan plan, ExecutionContext ctx);
}
