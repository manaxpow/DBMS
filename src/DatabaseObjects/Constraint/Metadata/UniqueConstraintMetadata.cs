public sealed class UniqueConstraintMetadata : ConstraintMetadata
{
    public override ConstraintType Type => ConstraintType.Unique;

    public IReadOnlyList<string> ColumnNames { get; }

    public UniqueConstraintMetadata(string name, IEnumerable<string> columnNames, bool isEnabled = true) : base(name, isEnabled)
    {
        ColumnNames = columnNames.ToList().AsReadOnly();
    }
}
