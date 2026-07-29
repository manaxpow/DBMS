public sealed class PrimaryKeyConstraintMetadata(string name, IEnumerable<string> columnNames, bool isEnabled = true)
    : ConstraintMetadata(name, isEnabled)
{
    public override ConstraintType Type => ConstraintType.PrimaryKey;

    public IReadOnlyList<string> ColumnNames { get; } = columnNames.ToList().AsReadOnly();
}
