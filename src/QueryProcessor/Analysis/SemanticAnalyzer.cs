using System;

public class SemanticAnalyzer : ISemanticAnalyzer
{
    private object _catalog; 

    public BoundStatement Analyze(SqlStatement statement, SemanticContext ctx)
    {
        return default;
    }

    public void ValidateTypes()
    {
    }
}
