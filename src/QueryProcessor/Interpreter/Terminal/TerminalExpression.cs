public abstract class TerminalExpression : Expression
{
    public abstract LogicalNode Interpret(InterpretationContext context);
}
