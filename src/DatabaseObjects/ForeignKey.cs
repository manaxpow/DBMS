using System;

public class ForeignKey : Constraint
{
    public string RefTable { get; set; }

    public new bool Validate(object parentKey)
    {
        throw new NotImplementedException();
    }

    public void ValidateParentDeletion(object parentKey)
    {
        throw new NotImplementedException();
    }
}
