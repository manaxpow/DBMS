using System.Collections.Generic;

public enum LogicalNodeType
{
    Project,
    Filter,
    Scan,
    Join
}

public class LogicalNode
{
    public LogicalNodeType Type { get; set; }
    public List<LogicalNode> Children { get; set; } = new List<LogicalNode>();
}
