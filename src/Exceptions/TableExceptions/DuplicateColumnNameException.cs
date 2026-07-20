using System;

public class DuplicateColumnNameException : Exception
{
    public DuplicateColumnNameException() : base() { }
    public DuplicateColumnNameException(string message) : base(message) { }
    public DuplicateColumnNameException(string message, Exception inner) : base(message, inner) { }
}
