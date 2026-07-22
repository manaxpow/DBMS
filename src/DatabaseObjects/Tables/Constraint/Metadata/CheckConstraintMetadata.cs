public sealed class CheckConstraintMetadata : ConstraintMetadata
{
    public override ConstraintType Type => ConstraintType.Check;
    public string Expression { get; }
    public CheckConstraintMetadata(string expression, string name, bool isEnabled = true) : base(name, isEnabled)
    {
        Expression = expression;
    }
}
