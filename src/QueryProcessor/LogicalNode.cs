public class LogicalNode
{
    public LogicalNodeType Type { get; set; }

    public List<LogicalNode> Children { get; set; } = new List<LogicalNode>();
}
