using System;
using Xunit;
using FluentAssertions;

namespace DBMS.UnitTests.DatabaseManager.Backups
{
    public class FullBackupTests
    {
        [Fact]
        public void ExtractData_ShouldExecuteWithoutError()
        {
            // Arrange
            var backup = new FullBackup();

            // Act
            Action act = () => backup.ExtractData();

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void FinalizeBackup_ShouldExecuteWithoutError()
        {
            // Arrange
            var backup = new FullBackup();

            // Act
            Action act = () => backup.FinalizeBackup();

            // Assert
            act.Should().NotThrow();
        }
    }
}
