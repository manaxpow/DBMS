using System;

public class Role
{
    public string RoleName { get; set; } = null!;

    public void AddPermission(Permission permission)
    {
        throw new NotImplementedException();
    }

    public void RemovePermission(Permission permission)
    {
        throw new NotImplementedException();
    }
}
