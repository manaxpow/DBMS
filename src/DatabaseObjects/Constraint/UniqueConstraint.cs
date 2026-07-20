public class UniqueConstraint : Constraint
{
    public IReadOnlyList<string> ColumnNames { get; set; }

    public UniqueConstraint(string name, IEnumerable<string> columnNames)
        : base(name)
    {
        ColumnNames = columnNames.ToList().AsReadOnly();
    }

    protected override bool Check(ConstraintContext context)
    {
        throw new NotImplementedException();
    }
}
