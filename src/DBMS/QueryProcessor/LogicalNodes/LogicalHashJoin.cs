public class LogicalHashJoin : LogicalNode
{
    public LogicalHashJoin()
    {
        this.Type = LogicalNodeType.Join;
    }
}
