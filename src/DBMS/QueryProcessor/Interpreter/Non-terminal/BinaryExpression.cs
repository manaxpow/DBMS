public class BinaryExpression : NonTerminalExpression
{
    public IExpression Left { get; set; } = null!;

    public IExpression Right { get; set; } = null!;

    public string Operator { get; set; } = null!;

    public override T Accept<T>(IExpressionVisitor<T> visitor) => throw new NotImplementedException();

    public override LogicalNode Interpret(InterpretationContext context)
    {
        throw new NotImplementedException();
    }
}
