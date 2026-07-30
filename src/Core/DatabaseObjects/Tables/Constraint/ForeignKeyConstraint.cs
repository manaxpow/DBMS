public class ForeignKeyConstraint(
    string name,
    string childColumnName,
    string referencedTableName,
    string referencedColumnName,
    IReferentialAction onDelete,
    IReferentialAction onUpdate)
    : Constraint(name)
{
    public string ChildColumnName { get; set; } = childColumnName;

    public string ReferencedTableName { get; set; } = referencedTableName;

    public string ReferencedColumnName { get; set; } = referencedColumnName;

    public IReferentialAction OnDelete { get; set; } = onDelete;

    public IReferentialAction OnUpdate { get; set; } = onUpdate;

    public bool IsNullable { get; set; }

    public void OnParentRowDeleted(Row parentRow, Table childTable)
    {
        this.OnDelete.Execute(parentRow, childTable);
    }

    protected override bool Check(ConstraintContext context)
    {
        throw new NotImplementedException();
    }
}
