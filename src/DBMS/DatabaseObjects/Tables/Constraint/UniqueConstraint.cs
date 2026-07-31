public class UniqueConstraint(string name, IEnumerable<string> columnNames)
    : Constraint(name)
{
    public IReadOnlyList<string> ColumnNames { get; set; } = columnNames.ToList().AsReadOnly();

    protected override bool Check(ConstraintContext context)
    {
        throw new NotImplementedException();
    }
}
