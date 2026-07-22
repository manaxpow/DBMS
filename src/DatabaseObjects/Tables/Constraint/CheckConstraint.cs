using System;

public class CheckConstraint : Constraint
{
    public Func<Row, bool> Predicate { get; set; }

    public CheckConstraint(string name, Func<Row, bool> predicate)
        : base(name)
    {
        Predicate = predicate;
    }

    protected override bool Check(ConstraintContext context)
    {
        throw new NotImplementedException();    
    }
}
