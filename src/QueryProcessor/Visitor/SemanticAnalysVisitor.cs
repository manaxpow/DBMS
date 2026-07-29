public class SemanticAnalysVisitor : IExpressionVisitor<bool>
{
    public bool Visit(IExpression expression) => throw new NotImplementedException();

    public bool Visit(ColumnExpression expression) => throw new NotImplementedException();

    public bool Visit(LiteralExpression expression) => throw new NotImplementedException();

    public bool Visit(BinaryExpression expression) => throw new NotImplementedException();

    public bool Visit(WhereExpression expression) => throw new NotImplementedException();

    public bool Visit(SelectExpression expression) => throw new NotImplementedException();
}
