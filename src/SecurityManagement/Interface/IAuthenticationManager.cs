using System;

public interface IAuthenticationManager
{
    AuthenticationResult Authenticate(Credential cred);
    SessionId CreateLoginSession(UserId userId);
}
