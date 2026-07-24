using System;
using Xunit;
using FluentAssertions;

public class SemanticAnalyzerTests
{
    [Trait("Category", "Important")]
    [Fact]
    public void Analyze_WhenASTIsValid_ShouldReturnLogicalPlan()
    {
        // Arrange
        var analyzer = new SemanticAnalyzer();
        var ast = new AST();

        // Act
        var plan = analyzer.Analyze(ast);

        // Assert
        plan.Should().NotBeNull();
        plan.IsValidated.Should().BeTrue();
    }

    [Fact]
    public void Analyze_WhenTableDoesNotExist_ShouldThrow()
    {
        // Arrange
        var analyzer = new SemanticAnalyzer();
        var ast = new AST();

        // Act
        Action act = () => analyzer.Analyze(ast);

        // Assert
        act.Should().Throw<SemanticException>();
    }
}
