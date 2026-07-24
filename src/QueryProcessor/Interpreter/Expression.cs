public interface Expression
{
    LogicalNode Interpret(InterpretationContext context);
    T Accept<T>(IExpressionVisitor<T> visitor);
}
