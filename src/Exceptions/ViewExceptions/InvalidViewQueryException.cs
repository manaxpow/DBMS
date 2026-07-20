using System;

public class InvalidViewQueryException : Exception
{
    public InvalidViewQueryException() : base() { }
    public InvalidViewQueryException(string message) : base(message) { }
    public InvalidViewQueryException(string message, Exception inner) : base(message, inner) { }
}
