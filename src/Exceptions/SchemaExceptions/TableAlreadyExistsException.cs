using System;

public class TableAlreadyExistsException : Exception
{
    public TableAlreadyExistsException()
        : base()
    {
    }

    public TableAlreadyExistsException(string message)
        : base(message)
    {
    }

    public TableAlreadyExistsException(string message, Exception inner)
        : base(message, inner)
    {
    }
}
