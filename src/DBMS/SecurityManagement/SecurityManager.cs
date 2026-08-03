using System;

public class SecurityManager : IServerComponent
{
    public void Start(object config)
    {
        throw new NotImplementedException();
    }

    public void Stop()
    {
        throw new NotImplementedException();
    }

    public object Authenticate(string username, string password)
    {
        throw new NotImplementedException();
    }

    public bool Authorize(object userToken, string operation, string resource)
    {
        throw new NotImplementedException();
    }

    public void AssignRole(int userId, int roleId)
    {
        throw new NotImplementedException();
    }

    public void RevokeRole(int userId, int roleId)
    {
        throw new NotImplementedException();
    }

    private string HashPassword(string password, string salt)
    {
        throw new NotImplementedException();
    }
}
