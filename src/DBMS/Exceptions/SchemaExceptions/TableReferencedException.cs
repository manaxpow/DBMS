using System;

public class TableReferencedException : Exception
{
    public TableReferencedException()
        : base()
    {
    }

    public TableReferencedException(string message)
        : base(message)
    {
    }

    public TableReferencedException(string message, Exception inner)
        : base(message, inner)
    {
    }
}
