using System;

public interface IAuthorizationManager
{
    bool CheckPermission(UserId userId, SecuredResource res, Privilege priv);
    void GrantPermission(UserId userId, SecuredResource res, Privilege priv);
}
