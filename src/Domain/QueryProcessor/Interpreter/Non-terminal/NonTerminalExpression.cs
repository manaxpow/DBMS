public abstract class NonTerminalExpression : IExpression
{
    public abstract T Accept<T>(IExpressionVisitor<T> visitor);

    public abstract LogicalNode Interpret(InterpretationContext context);
}
