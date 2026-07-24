public class WhereExpression : NonTerminalExpression
{
    public Expression Condition { get; set; }
    public override LogicalNode Interpret(InterpretationContext context) { throw new NotImplementedException(); }
}
