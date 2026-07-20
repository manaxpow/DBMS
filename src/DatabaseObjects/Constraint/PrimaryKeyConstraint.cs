using System;
using System.Collections.Generic;
using System.Linq;

public class PrimaryKeyConstraint : Constraint
{
    public IReadOnlyList<string> ColumnNames { get; set; }

    public PrimaryKeyConstraint(string name, IEnumerable<string> columnNames)
        : base(name)
    {
        ColumnNames = columnNames.ToList().AsReadOnly();
    }

    protected override bool Check(ConstraintContext context)
    {
        throw new NotImplementedException();    
    }
}
