using System;
using Xunit;

namespace DBMS.UnitTests.DatabaseManager.Commands
{
    public class RenameDatabaseCommandTests
    {
        [Fact]
        public void Execute_WhenDatabaseExists_ShouldRenameDatabaseAndReturnSuccess()
        {
            // TODO: Implement test
            throw new NotImplementedException();
        }

        [Fact]
        public void Execute_WhenDatabaseDoesNotExist_ShouldReturnFailure()
        {
            // TODO: Implement test
            throw new NotImplementedException();
        }

        [Fact]
        public void Execute_WhenNewNameAlreadyExists_ShouldReturnFailure()
        {
            // TODO: Implement test
            throw new NotImplementedException();
        }
    }
}
