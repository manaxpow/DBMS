using System;
using Xunit;
using DBMS.ReplicationCluster;

public class ReplicationManagerTests
{
    [Fact]
    public void Replicate_WhenFollowerIsAvailable_ShouldSendLogRecords()
    {
        throw new NotImplementedException();
    }

    [Fact]
    public void Replicate_WhenFollowerFails_ShouldRetry()
    {
        throw new NotImplementedException();
    }

    [Fact]
    public void Commit_WhenQuorumIsNotReached_ShouldFail()
    {
        throw new NotImplementedException();
    }

}
