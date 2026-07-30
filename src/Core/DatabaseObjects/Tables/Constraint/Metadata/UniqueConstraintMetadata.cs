public sealed class UniqueConstraintMetadata(string name, IEnumerable<string> columnNames, bool isEnabled = true)
    : ConstraintMetadata(name, isEnabled)
{
    public override ConstraintType Type => ConstraintType.Unique;

    public IReadOnlyList<string> ColumnNames { get; } = columnNames.ToList().AsReadOnly();
}
