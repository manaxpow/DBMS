public interface IExpressionVisitor<T>
{
    T Visit(Expression expression);
    T Visit(ColumnExpression expression);
    T Visit(LiteralExpression expression);
    T Visit(BinaryExpression expression);
    T Visit(WhereExpression expression);
    T Visit(SelectExpression expression);
}
