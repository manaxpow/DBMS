using System;
using Xunit;
using FluentAssertions;

public class QueryProcessorTests
{
    [Trait("Category", "Important")]
    [Fact]
    public void ExecuteQuery_WhenGivenSQL_ShouldOrchestrateComponentsAndReturnResultSet()
    {
        // Arrange
        var qp = new QueryProcessor();
        var sql = "SELECT * FROM table";

        // Act
        var result = qp.ExecuteQuery(sql);

        // Assert
        result.Should().NotBeNull();
        result.Rows.Should().NotBeNull();
        result.Status.Should().Be(ExecutionStatus.Success);
    }
}
