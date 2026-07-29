using System;

public class DuplicateTableNameException : Exception
{
    public DuplicateTableNameException()
        : base()
    {
    }

    public DuplicateTableNameException(string message)
        : base(message)
    {
    }

    public DuplicateTableNameException(string message, Exception inner)
        : base(message, inner)
    {
    }
}
