using System;

public interface ISemanticAnalyzer
{
    BoundStatement Analyze(SqlStatement statement, SemanticContext ctx);
}
