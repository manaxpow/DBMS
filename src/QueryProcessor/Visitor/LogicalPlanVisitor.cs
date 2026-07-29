public class LogicalPlanVisitor : IExpressionVisitor<LogicalNode>
{
    public LogicalNode Visit(IExpression expression) => throw new NotImplementedException();

    public LogicalNode Visit(ColumnExpression expression) => throw new NotImplementedException();

    public LogicalNode Visit(LiteralExpression expression) => throw new NotImplementedException();

    public LogicalNode Visit(BinaryExpression expression) => throw new NotImplementedException();

    public LogicalNode Visit(WhereExpression expression) => throw new NotImplementedException();

    public LogicalNode Visit(SelectExpression expression) => throw new NotImplementedException();
}
