public abstract class NonTerminalExpression : Expression
{
    public abstract T Accept<T>(IExpressionVisitor<T> visitor);

    public abstract LogicalNode Interpret(InterpretationContext context);
}
