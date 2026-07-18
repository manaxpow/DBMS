using System;

public class ForeignKeyConstraintException : Exception
{
    public ForeignKeyConstraintException() : base() { }
    public ForeignKeyConstraintException(string message) : base(message) { }
    public ForeignKeyConstraintException(string message, Exception inner) : base(message, inner) { }
}
