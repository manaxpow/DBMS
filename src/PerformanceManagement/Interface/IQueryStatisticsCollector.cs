using System;using System.Collections.Generic;

public interface IQueryStatisticsCollector
{
    void RecordQueryExecution(QueryExecutionStatistics stats);
    List<QueryExecutionStatistics> GetSlowQueries(TimeSpan threshold);
}
