public class LiteralExpression : TerminalExpression
{
    public object Value { get; set; }
    public override LogicalNode Interpret(InterpretationContext context) { throw new NotImplementedException(); }
}
