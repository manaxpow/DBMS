public interface Expression
{
    LogicalNode Interpret(InterpretationContext context);
}
