using System;

public class ClusterNode
{
    public string NodeId { get; set; } = null!;

    public void ReceiveHeartbeat()
    {
        throw new NotImplementedException();
    }

    public void MarkUnavailable()
    {
        throw new NotImplementedException();
    }

    public void Create()
    {
        throw new NotImplementedException();
    }
}
