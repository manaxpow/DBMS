using System.Collections.Generic;

public enum ExecutionStatus
{
    Success,
    Error,
    InProgress
}

public class ResultSet
{
    public List<Row> Rows { get; set; } = new List<Row>();
    public ExecutionStatus Status { get; set; }
}
