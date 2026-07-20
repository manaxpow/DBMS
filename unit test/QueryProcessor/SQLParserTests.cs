using System;
using Xunit;

public class SQLParserTests
{
    [Fact]
    public void Parse_WhenSelectStatementIsValid_ShouldReturnAST()
    {
        throw new NotImplementedException();
    }

    [Fact]
    public void Parse_WhenStatementIsIncomplete_ShouldThrowSyntaxError()
    {
        throw new NotImplementedException();
    }

    [Fact]
    public void Parse_WhenTokensAreEmpty_ShouldRejectInput()
    {
        throw new NotImplementedException();
    }


    [Fact]
    public void Parse_WhenInsertStatementIsValid_ShouldReturnAST()
    {
        throw new NotImplementedException();
    }

    [Fact]
    public void Parse_WhenUpdateStatementIsValid_ShouldReturnAST()
    {
        throw new NotImplementedException();
    }

    [Fact]
    public void Parse_WhenDeleteStatementIsValid_ShouldReturnAST()
    {
        throw new NotImplementedException();
    }

    [Fact]
    public void Parse_WhenUnexpectedTokenAppears_ShouldReportTokenPosition()
    {
        throw new NotImplementedException();
    }

    [Fact]
    public void Parse_WhenExpressionIsNested_ShouldPreservePrecedence()
    {
        throw new NotImplementedException();
    }

    [Fact]
    public void Parse_WhenClauseOrderIsInvalid_ShouldThrowSyntaxError()
    {
        throw new NotImplementedException();
    }
}
