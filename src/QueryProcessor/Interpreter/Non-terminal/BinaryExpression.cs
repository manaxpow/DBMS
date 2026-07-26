public class BinaryExpression : NonTerminalExpression
{
    public Expression Left { get; set; }
    public Expression Right { get; set; }
    public string Operator { get; set; }

    public override T Accept<T>(IExpressionVisitor<T> visitor) => throw new NotImplementedException();

    public override LogicalNode Interpret(InterpretationContext context) { throw new NotImplementedException(); }
}

