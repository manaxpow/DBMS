using System;

public class InvalidColumnNameException : Exception
{
    public InvalidColumnNameException() : base() { }
    public InvalidColumnNameException(string message) : base(message) { }
    public InvalidColumnNameException(string message, Exception inner) : base(message, inner) { }
}
