using System;

public class ConstraintViolationException : Exception
{
    public ConstraintViolationException() : base() { }
    public ConstraintViolationException(string message) : base(message) { }
    public ConstraintViolationException(string message, Exception inner) : base(message, inner) { }
}
