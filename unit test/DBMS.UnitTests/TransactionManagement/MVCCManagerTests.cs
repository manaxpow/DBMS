using System;
using Xunit;
using DBMS.TransactionManagement;

namespace DBMS.UnitTests.TransactionManagement
{
    public class MVCCManagerTests
    {
        [Fact]
        public void CreateVersion_WhenRowChanges_ShouldCreateNewVersion()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void ReadVersion_WhenVersionIsVisible_ShouldReturnVersion()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void Cleanup_WhenVersionIsObsolete_ShouldRemoveVersion()
        {
            throw new NotImplementedException();
        }

    }
}

