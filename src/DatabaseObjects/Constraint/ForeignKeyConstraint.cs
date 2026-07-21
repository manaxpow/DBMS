
public class ForeignKeyConstraint : Constraint
{
    public string ChildColumnName { get; set; }
    public string ReferencedTableName { get; set; }
    public string ReferencedColumnName { get; set; }
    public IReferentialAction OnDelete { get; set; }
    public IReferentialAction OnUpdate { get; set; }
    public bool IsNullable { get; set; }

    public ForeignKeyConstraint(
        string name, string childColumnName, string referencedTableName,
        string referencedColumnName, IReferentialAction onDelete, IReferentialAction onUpdate)
        : base(name)
    {
        ChildColumnName = childColumnName;
        ReferencedTableName = referencedTableName;
        ReferencedColumnName = referencedColumnName;
        OnDelete = onDelete;
        OnUpdate = onUpdate;
    }

    protected override bool Check(ConstraintContext context)
    {
        throw new NotImplementedException();
    }

    public void OnParentRowDeleted(Row parentRow, Table childTable)
    {
        OnDelete.Execute(parentRow, childTable);
    }
}
