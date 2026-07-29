using System;

public class RowSchemaMismatchException : Exception
{
    public RowSchemaMismatchException()
        : base()
    {
    }

    public RowSchemaMismatchException(string message)
        : base(message)
    {
    }

    public RowSchemaMismatchException(string message, Exception inner)
        : base(message, inner)
    {
    }
}
