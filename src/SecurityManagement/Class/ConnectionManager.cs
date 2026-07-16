using System;using System.Collections.Generic;

public class ConnectionManager : IConnectionManager
{
    private object _registry;
    private IAuthenticationManager _authenticationManager;

    public ConnectionManager(IAuthenticationManager authenticationManager)
    {
        _authenticationManager = authenticationManager;
    }

    public ConnectionId OpenConnection(ConnectionContext ctx)
    {
        return default;
    }

    public void CloseConnection(ConnectionId connId)
    {
    }

    public List<ConnectionId> GetActiveConnections()
    {
        return default;
    }

    public void LimitConnections()
    {
    }
}
