public sealed class PrimaryKeyConstraintMetadata : ConstraintMetadata
{
    public override ConstraintType Type => ConstraintType.PrimaryKey;

    public IReadOnlyList<string> ColumnNames { get; }

    public PrimaryKeyConstraintMetadata(string name, IEnumerable<string> columnNames, bool isEnabled = true) : base(name, isEnabled)
    {
        ColumnNames = columnNames.ToList().AsReadOnly();
    }
}
