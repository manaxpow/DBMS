public abstract class NonTerminalExpression : Expression
{
    public abstract LogicalNode Interpret(InterpretationContext context);
}
