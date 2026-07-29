using System;
using Xunit;
using FluentAssertions;

namespace DBMS.UnitTests.DatabaseManager.Backups
{
    public class IncrementalBackupTests
    {
        [Fact]
        public void ExtractData_ShouldExecuteWithoutError()
        {
            // Arrange
            var backup = new IncrementalBackup();

            // Act
            Action act = () => backup.ExtractData();

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void FinalizeBackup_ShouldExecuteWithoutError()
        {
            // Arrange
            var backup = new IncrementalBackup();

            // Act
            Action act = () => backup.FinalizeBackup();

            // Assert
            act.Should().NotThrow();
        }
    }
}
