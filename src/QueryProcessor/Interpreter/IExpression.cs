public interface IExpression
{
    LogicalNode Interpret(InterpretationContext context);

    T Accept<T>(IExpressionVisitor<T> visitor);
}
