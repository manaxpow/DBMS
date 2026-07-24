public class InterpretationContext
{
    public InterpretationContext() { }

    public virtual LogicalNode ResolveTable(string name) { throw new NotImplementedException(); }
    public virtual LogicalNode ResolveColumn(string name) { throw new NotImplementedException(); }
}
