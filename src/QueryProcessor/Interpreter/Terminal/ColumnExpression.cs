public class ColumnExpression : TerminalExpression
{
    public string ColumnName { get; set; }
    public override LogicalNode Interpret(InterpretationContext context) { throw new NotImplementedException(); }
}
