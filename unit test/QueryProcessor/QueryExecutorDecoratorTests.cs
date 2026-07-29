using System;
using FluentAssertions;
using Xunit;
using NSubstitute;

public class QueryExecutorDecoratorTests
{
    [Fact]
    public void QueryExecutionLoggerDecorator_Execute_ShouldLogStartAndSuccess()
    {
        // Arrange
        var mockExecutor = Substitute.For<IQueryExecutor>();
        var mockLogger = Substitute.For<ILogger>();
        var plan = new PhysicalPlan();
        var expectedResult = new ResultSet();

        mockExecutor.Execute(plan).Returns(expectedResult);

        var decorator = new QueryExecutionLoggerDecorator(mockExecutor, mockLogger);

        // Act
        var result = decorator.Execute(plan);

        // Assert
        result.Should().Be(expectedResult);
        mockLogger.Received(1).Log(Arg.Is<string>(s => s.Contains("[Start]")));
        mockLogger.Received(1).Log(Arg.Is<string>(s => s.Contains("[Success]")));
        mockExecutor.Received(1).Execute(plan);
    }

    [Fact]
    public void ProfilingDecorator_Execute_ShouldExecuteInner()
    {
        // Arrange
        var mockExecutor = Substitute.For<IQueryExecutor>();
        var plan = new PhysicalPlan();
        var expectedResult = new ResultSet();

        mockExecutor.Execute(plan).Returns(expectedResult);

        var decorator = new ProfilingDecorator(mockExecutor);

        // Act
        var result = decorator.Execute(plan);

        // Assert
        result.Should().Be(expectedResult);
        mockExecutor.Received(1).Execute(plan);
    }

    [Fact]
    public void AuditDecorator_Execute_ShouldRecordStartAndEnd()
    {
        // Arrange
        var mockExecutor = Substitute.For<IQueryExecutor>();
        var mockAuditLogger = Substitute.For<IAuditLogger>();
        var plan = new PhysicalPlan();
        var expectedResult = new ResultSet();

        mockExecutor.Execute(plan).Returns(expectedResult);

        var decorator = new AuditDecorator(mockExecutor, mockAuditLogger);

        // Act
        var result = decorator.Execute(plan);

        // Assert
        result.Should().Be(expectedResult);
        mockAuditLogger.Received(1).Record(Arg.Is<string>(s => s.Contains("requested")));
        mockAuditLogger.Received(1).Record(Arg.Is<string>(s => s.Contains("completed")));
        mockExecutor.Received(1).Execute(plan);
    }
}
