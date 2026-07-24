using System;
using Xunit;
using FluentAssertions;

public class LexerTests
{
    [Trait("Category", "Important")]
    [Fact]
    public void Tokenize_WhenSQLIsValid_ShouldReturnTokens()
    {
        // Arrange
        var lexer = new Lexer();
        var sql = "SELECT * FROM table";

        // Act
        var tokens = lexer.Tokenize(sql);

        // Assert
        tokens.Should().NotBeNull();
        tokens.Count.Should().BeGreaterThan(0);
        tokens[0].Type.Should().Be(TokenType.Keyword);
    }

    [Fact]
    public void Tokenize_WhenInputContainsWhitespace_ShouldIgnoreWhitespace()
    {
        throw new NotImplementedException();
    }

    [Trait("Category", "Important")]
    [Fact]
    public void Tokenize_WhenTokenIsInvalid_ShouldThrow()
    {
        // Arrange
        var lexer = new Lexer();
        var sql = "SELECT @#$ FROM table";

        // Act
        Action act = () => lexer.Tokenize(sql);

        // Assert
        act.Should().Throw<LexerException>();
    }

    [Fact]
    public void Tokenize_WhenInputContainsComments_ShouldIgnoreComments()
    {
        throw new NotImplementedException();
    }

    [Fact]
    public void Tokenize_WhenKeywordIsProvided_ShouldReturnKeywordToken()
    {
        throw new NotImplementedException();
    }

    [Fact]
    public void Tokenize_WhenIdentifierIsProvided_ShouldReturnIdentifierToken()
    {
        throw new NotImplementedException();
    }

    [Fact]
    public void Tokenize_WhenNumberLiteralIsProvided_ShouldReturnNumberToken()
    {
        throw new NotImplementedException();
    }

    [Fact]
    public void Tokenize_WhenStringLiteralIsProvided_ShouldReturnStringToken()
    {
        throw new NotImplementedException();
    }

    [Fact]
    public void Tokenize_WhenOperatorIsProvided_ShouldReturnOperatorToken()
    {
        throw new NotImplementedException();
    }

    [Fact]
    public void Tokenize_WhenStringLiteralIsUnterminated_ShouldThrow()
    {
        throw new NotImplementedException();
    }
}
