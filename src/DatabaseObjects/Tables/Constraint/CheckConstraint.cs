using System;

public class CheckConstraint(string name, Func<Row, bool> predicate)
    : Constraint(name)
{
    public Func<Row, bool> Predicate { get; set; } = predicate;

    protected override bool Check(ConstraintContext context)
    {
        throw new NotImplementedException();
    }
}
