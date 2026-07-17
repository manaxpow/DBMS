using System;
using Xunit;
using DBMS.RecoveryManagement;

namespace DBMS.UnitTests.RecoveryManagement
{
    public class WALManagerTests
    {
        [Fact]
        public void Append_WhenRecordIsValid_ShouldAssignLSN()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void Flush_WhenTargetLSNExists_ShouldPersistRecords()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void Append_WhenSequenceIsInvalid_ShouldThrow()
        {
            throw new NotImplementedException();
        }

    }
}

