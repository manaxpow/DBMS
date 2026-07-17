using System;

public class ColumnTypeMismatchException : Exception
{
    public ColumnTypeMismatchException() : base() { }
    public ColumnTypeMismatchException(string message) : base(message) { }
    public ColumnTypeMismatchException(string message, Exception inner) : base(message, inner) { }
}
