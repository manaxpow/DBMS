using System;

public class AuthenticationManager : IAuthenticationManager
{
    private object _hasher;
    private IPrincipalManager _principalManager;

    public AuthenticationManager(IPrincipalManager principalManager)
    {
        _principalManager = principalManager;
    }

    public AuthenticationResult Authenticate(Credential cred)
    {
        return default;
    }

    public SessionId CreateLoginSession(UserId userId)
    {
        return default;
    }

    public void ValidateToken()
    {
    }
}
