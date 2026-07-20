
public class ForeignKeyConstraint : Constraint
{
    public string ChildColumnName { get; set; }
    public string ReferencedTableName { get; set; }
    public string ReferencedColumnName { get; set; }
    public ReferentialAction OnDelete { get; set; }
    public ReferentialAction OnUpdate { get; set; }
    public bool IsNullable { get; set; }

    public ForeignKeyConstraint(string name, string childColumnName, string referencedTableName, string referencedColumnName)
        : base(name)
    {
        ChildColumnName = childColumnName;
        ReferencedTableName = referencedTableName;
        ReferencedColumnName = referencedColumnName;
    }

    protected override bool Check(ConstraintContext context)
    {
        throw new NotImplementedException();
    }
}
