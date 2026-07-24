public class SelectExpression : NonTerminalExpression
{
    public List<Expression> Columns { get; set; }
    public Expression Where { get; set; }
    public Expression From { get; set; }

    public override T Accept<T>(IExpressionVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }

    public override LogicalNode Interpret(InterpretationContext context) { throw new NotImplementedException(); }
}
