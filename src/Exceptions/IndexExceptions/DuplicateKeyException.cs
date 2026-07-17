using System;

public class DuplicateKeyException : Exception
{
    public DuplicateKeyException() : base() { }
    public DuplicateKeyException(string message) : base(message) { }
    public DuplicateKeyException(string message, Exception inner) : base(message, inner) { }
}
