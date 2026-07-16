using System;

public class PrincipalManager : IPrincipalManager
{
    private object _repo;

    public UserId CreateUser(string username, string password)
    {
        return default;
    }

    public void AssignRole(UserId userId, RoleId roleId)
    {
    }

    public void UpdateStatus()
    {
    }
}
