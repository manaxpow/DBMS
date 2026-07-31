using System;
using System.Collections.Generic;
using System.Linq;

public class PrimaryKeyConstraint(string name, IEnumerable<string> columnNames)
    : Constraint(name)
{
    public IReadOnlyList<string> ColumnNames { get; set; } = columnNames.ToList().AsReadOnly();

    protected override bool Check(ConstraintContext context)
    {
        throw new NotImplementedException();
    }
}
