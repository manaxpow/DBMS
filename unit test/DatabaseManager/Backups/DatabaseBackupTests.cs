using System;
using System.Collections.Generic;
using Xunit;
using FluentAssertions;

namespace DBMS.UnitTests.DatabaseManager.Backups
{
    public class TestableDatabaseBackup : DatabaseBackup
    {
        public List<string> ExecutionLog { get; } = new List<string>();

        public override void InitializeBackup()
        {
            ExecutionLog.Add("InitializeBackup");
        }

        public override void ExtractData()
        {
            ExecutionLog.Add("ExtractData");
        }

        public override void CompressData()
        {
            ExecutionLog.Add("CompressData");
        }

        public override void FinalizeBackup()
        {
            ExecutionLog.Add("FinalizeBackup");
        }
    }

    public class DatabaseBackupTests
    {
        [Fact]
        public void ExecuteBackup_ShouldCallStepsInCorrectOrder()
        {
            // Arrange
            var backup = new TestableDatabaseBackup();

            // Act
            backup.ExecuteBackup();

            // Assert
            backup.ExecutionLog.Should().HaveCount(4);
            backup.ExecutionLog[0].Should().Be("InitializeBackup");
            backup.ExecutionLog[1].Should().Be("ExtractData");
            backup.ExecutionLog[2].Should().Be("CompressData");
            backup.ExecutionLog[3].Should().Be("FinalizeBackup");
        }
    }
}
