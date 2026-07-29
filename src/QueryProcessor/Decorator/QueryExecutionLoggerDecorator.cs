using System;

public interface ILogger
{
    void Log(string message);
}

public class QueryExecutionLoggerDecorator : QueryExecutorDecorator
{
    private readonly ILogger logger;

    public QueryExecutionLoggerDecorator(IQueryExecutor innerExecutor, ILogger logger)
        : base(innerExecutor)
    {
        this.logger = logger;
    }

    public override ResultSet Execute(PhysicalPlan plan)
    {
        // Implementation would log and call base
        this.logger.Log("[Start] Executing query");
        var result = this.InnerExecutor.Execute(plan);
        this.logger.Log("[Success] Execution successful");
        return result;
    }
}
