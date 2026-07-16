using System;

public class AuthorizationManager : IAuthorizationManager
{
    private object _evaluator;
    private IPrincipalManager _principalManager;

    public AuthorizationManager(IPrincipalManager principalManager)
    {
        _principalManager = principalManager;
    }

    public bool CheckPermission(UserId userId, SecuredResource res, Privilege priv)
    {
        return default;
    }

    public void GrantPermission(UserId userId, SecuredResource res, Privilege priv)
    {
    }

    public void ResolveRoles()
    {
    }
}
