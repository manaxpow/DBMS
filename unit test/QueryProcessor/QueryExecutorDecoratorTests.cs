using System;
using FluentAssertions;
using Xunit;
using Moq;

public class QueryExecutorDecoratorTests
{
    [Fact]
    public void QueryExecutionLoggerDecorator_Execute_ShouldLogStartAndSuccess()
    {
        // Arrange
        var mockExecutor = new Mock<IQueryExecutor>();
        var mockLogger = new Mock<ILogger>();
        var plan = new PhysicalPlan();
        var expectedResult = new QueryResult();

        mockExecutor.Setup(e => e.Execute(plan)).Returns(expectedResult);

        var decorator = new QueryExecutionLoggerDecorator(mockExecutor.Object, mockLogger.Object);

        // Act
        var result = decorator.Execute(plan);

        // Assert
        result.Should().Be(expectedResult);
        mockLogger.Verify(l => l.Log(It.Is<string>(s => s.Contains("[Start]"))), Times.Once);
        mockLogger.Verify(l => l.Log(It.Is<string>(s => s.Contains("[Success]"))), Times.Once);
        mockExecutor.Verify(e => e.Execute(plan), Times.Once);
    }

    [Fact]
    public void ProfilingDecorator_Execute_ShouldExecuteInner()
    {
        // Arrange
        var mockExecutor = new Mock<IQueryExecutor>();
        var plan = new PhysicalPlan();
        var expectedResult = new QueryResult();

        mockExecutor.Setup(e => e.Execute(plan)).Returns(expectedResult);

        var decorator = new ProfilingDecorator(mockExecutor.Object);

        // Act
        var result = decorator.Execute(plan);

        // Assert
        result.Should().Be(expectedResult);
        mockExecutor.Verify(e => e.Execute(plan), Times.Once);
    }

    [Fact]
    public void AuditDecorator_Execute_ShouldRecordStartAndEnd()
    {
        // Arrange
        var mockExecutor = new Mock<IQueryExecutor>();
        var mockAuditLogger = new Mock<IAuditLogger>();
        var plan = new PhysicalPlan();
        var expectedResult = new QueryResult();

        mockExecutor.Setup(e => e.Execute(plan)).Returns(expectedResult);

        var decorator = new AuditDecorator(mockExecutor.Object, mockAuditLogger.Object);

        // Act
        var result = decorator.Execute(plan);

        // Assert
        result.Should().Be(expectedResult);
        mockAuditLogger.Verify(a => a.Record(It.Is<string>(s => s.Contains("requested"))), Times.Once);
        mockAuditLogger.Verify(a => a.Record(It.Is<string>(s => s.Contains("completed"))), Times.Once);
        mockExecutor.Verify(e => e.Execute(plan), Times.Once);
    }
}
