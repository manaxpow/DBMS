using System;

public class ColumnAlreadyExistsException : Exception
{
    public ColumnAlreadyExistsException()
        : base()
    {
    }

    public ColumnAlreadyExistsException(string message)
        : base(message)
    {
    }

    public ColumnAlreadyExistsException(string message, Exception inner)
        : base(message, inner)
    {
    }
}
