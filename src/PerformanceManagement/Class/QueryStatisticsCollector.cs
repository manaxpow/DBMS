using System;using System.Collections.Generic;

public class QueryStatisticsCollector : IQueryStatisticsCollector
{
    private object _repo;

    public void RecordQueryExecution(QueryExecutionStatistics stats)
    {
    }

    public List<QueryExecutionStatistics> GetSlowQueries(TimeSpan threshold)
    {
        return default;
    }

    public void DetectSlowQueries()
    {
    }
}
