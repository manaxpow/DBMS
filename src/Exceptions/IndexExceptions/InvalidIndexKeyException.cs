using System;

public class InvalidIndexKeyException : Exception
{
    public InvalidIndexKeyException() : base() { }
    public InvalidIndexKeyException(string message) : base(message) { }
    public InvalidIndexKeyException(string message, Exception inner) : base(message, inner) { }
}
