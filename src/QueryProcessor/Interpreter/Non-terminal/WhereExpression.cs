public class WhereExpression : NonTerminalExpression
{
    public IExpression Condition { get; set; } = null!;

    public override T Accept<T>(IExpressionVisitor<T> visitor) => throw new NotImplementedException();

    public override LogicalNode Interpret(InterpretationContext context)
    {
        throw new NotImplementedException();
    }
}
