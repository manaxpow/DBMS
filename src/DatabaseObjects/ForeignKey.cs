using System;

public class ForeignKey : Constraint
{
    public string RefTable { get; set; }

    public new void Validate(object value)
    {
        throw new NotImplementedException();
    }

    public void DeleteParent()
    {
        throw new NotImplementedException();
    }
}
