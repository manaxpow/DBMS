public class LiteralExpression : TerminalExpression
{
    public object Value { get; set; }

    public override T Accept<T>(IExpressionVisitor<T> visitor) => throw new NotImplementedException();

    public override LogicalNode Interpret(InterpretationContext context) { throw new NotImplementedException(); }
}

