public class SelectExpression : NonTerminalExpression
{
    public List<Expression> Columns { get; set; }
    public Expression Where { get; set; }
    public Expression From { get; set; }
    public override LogicalNode Interpret(InterpretationContext context) { throw new NotImplementedException(); }
}
