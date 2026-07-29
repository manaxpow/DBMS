using System;

public class InvalidColumnValueException : Exception
{
    public InvalidColumnValueException()
        : base()
    {
    }

    public InvalidColumnValueException(string message)
        : base(message)
    {
    }

    public InvalidColumnValueException(string message, Exception inner)
        : base(message, inner)
    {
    }
}
