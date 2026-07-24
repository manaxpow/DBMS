public class TableExpression : TerminalExpression
{
    public string TableName { get; set; }

    public override T Accept<T>(IExpressionVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }

    public override LogicalNode Interpret(InterpretationContext context) { throw new NotImplementedException(); }
}
