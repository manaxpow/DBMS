using System;

public class InvalidProcedureBodyException : Exception
{
    public InvalidProcedureBodyException() : base() { }
    public InvalidProcedureBodyException(string message) : base(message) { }
    public InvalidProcedureBodyException(string message, Exception inner) : base(message, inner) { }
}
