using System;

public interface ILogger
{
    void Log(string message);
}

public class QueryExecutionLoggerDecorator : QueryExecutorDecorator
{
    private readonly ILogger _logger;

    public QueryExecutionLoggerDecorator(IQueryExecutor innerExecutor, ILogger logger)
        : base(innerExecutor)
    {
        this._logger = logger;
    }

    public override ResultSet Execute(PhysicalPlan plan)
    {
        // Implementation would log and call base
        this._logger.Log("[Start] Executing query");
        var result = this.InnerExecutor.Execute(plan);
        this._logger.Log("[Success] Execution successful");
        return result;
    }
}
