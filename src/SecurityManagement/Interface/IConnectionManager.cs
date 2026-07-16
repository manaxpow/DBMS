using System;using System.Collections.Generic;

public interface IConnectionManager
{
    ConnectionId OpenConnection(ConnectionContext ctx);
    void CloseConnection(ConnectionId connId);
    List<ConnectionId> GetActiveConnections();
}
