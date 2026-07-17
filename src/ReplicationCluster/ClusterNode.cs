using System;

namespace DBMS.ReplicationCluster
{
    public class ClusterNode
    {
        public string NodeId { get; set; }

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
}
