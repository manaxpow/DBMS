public sealed class ForeignKeyConstraintMetadata : ConstraintMetadata
{
    public override ConstraintType Type => ConstraintType.ForeignKey;

    public string ChildColumnName { get; }
    public string ReferencedTableName { get; }
    public string ReferencedColumnName { get; }

    public IReferentialAction OnDelete { get; }
    public IReferentialAction OnUpdate { get; }

    public ForeignKeyConstraintMetadata(string childColumnName, string name, string referencedTableName, string referencedColumnName,
        IReferentialAction onDelete, IReferentialAction onUpdate) : base(name)
    {
        ChildColumnName = childColumnName;
        ReferencedTableName = referencedTableName;
        ReferencedColumnName = referencedColumnName;
        OnDelete = onDelete;
        OnUpdate = onUpdate;
    }
}
