using System;

public interface IPrincipalManager
{
    UserId CreateUser(string username, string password);
    void AssignRole(UserId userId, RoleId roleId);
}
