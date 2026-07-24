using System;

public class QueryProcessorException : Exception
{
    public QueryProcessorException(string message) : base(message) { }
    public QueryProcessorException(string message, Exception innerException) : base(message, innerException) { }
}

public class LexerException : QueryProcessorException
{
    public LexerException(string message = "Lexer encountered an error.") : base(message) { }
}

public class SQLParserException : QueryProcessorException
{
    public SQLParserException(string message = "Parser encountered a syntax error.") : base(message) { }
}

public class SemanticException : QueryProcessorException
{
    public SemanticException(string message = "Semantic analysis failed.") : base(message) { }
}

public class QueryOptimizerException : QueryProcessorException
{
    public QueryOptimizerException(string message = "Optimizer failed to generate a plan.") : base(message) { }
}

public class QueryExecutorException : QueryProcessorException
{
    public QueryExecutorException(string message = "Query execution failed.") : base(message) { }
}
