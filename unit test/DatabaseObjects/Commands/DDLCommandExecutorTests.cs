using System;
using Xunit;
using FluentAssertions;

public class DDLCommandExecutorTests
{
    private class FakeDDLCommand : IDDLCommand
    {
        public DDLResult Execute()
        {
            return DDLResult.Success;
        }
    }

    [Fact]
    public void Execute_WhenCommandIsValid_ShouldReturnCommandResult()
    {
        // Arrange
        var executor = new DDLCommandExecutor();
        var command = new FakeDDLCommand();

        // Act
        var result = executor.Execute(command);

        // Assert
        result.Should().Be(DDLResult.Success);
    }
}
