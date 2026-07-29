using System;

public class Permission
{
    public string Action { get; set; } = null!;

    public bool Allows(string action, object resource)
    {
        throw new NotImplementedException();
    }
}
