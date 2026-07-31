public sealed class CheckConstraintMetadata(string expression, string name, bool isEnabled = true)
    : ConstraintMetadata(name, isEnabled)
{
    public override ConstraintType Type => ConstraintType.Check;

    public string IExpression { get; } = expression;
}
