public class TableExpression : TerminalExpression
{
    public string TableName { get; set; }
    public override LogicalNode Interpret(InterpretationContext context) { throw new NotImplementedException(); }
}
