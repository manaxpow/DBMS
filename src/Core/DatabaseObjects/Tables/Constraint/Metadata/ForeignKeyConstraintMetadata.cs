public sealed class ForeignKeyConstraintMetadata(
    string childColumnName,
    string name,
    string referencedTableName,
    string referencedColumnName,
    IReferentialAction onDelete,
    IReferentialAction onUpdate)
    : ConstraintMetadata(name)
{
    public override ConstraintType Type => ConstraintType.ForeignKey;

    public string ChildColumnName { get; } = childColumnName;

    public string ReferencedTableName { get; } = referencedTableName;

    public string ReferencedColumnName { get; } = referencedColumnName;

    public IReferentialAction OnDelete { get; } = onDelete;

    public IReferentialAction OnUpdate { get; } = onUpdate;
}
