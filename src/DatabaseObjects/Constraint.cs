using System;

public class Constraint
{
    public bool IsEnabled { get; set; }

    public bool Check(object value)
    {
        throw new NotImplementedException();
    }

    public bool Validate(object value)
    {
        throw new NotImplementedException();
    }

    public void Apply(object value)
    {
        throw new NotImplementedException();
    }
}
