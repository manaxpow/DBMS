public class SelectExpression : NonTerminalExpression
{
    public List<IExpression> Columns { get; set; } = null!;

    public IExpression Where { get; set; } = null!;

    public IExpression From { get; set; } = null!;

    public override T Accept<T>(IExpressionVisitor<T> visitor) => throw new NotImplementedException();

    public override LogicalNode Interpret(InterpretationContext context)
    {
        throw new NotImplementedException();
    }
}
