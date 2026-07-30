using System;

public class ViewReferencedException : Exception
{
    public ViewReferencedException()
        : base()
    {
    }

    public ViewReferencedException(string message)
        : base(message)
    {
    }

    public ViewReferencedException(string message, Exception inner)
        : base(message, inner)
    {
    }
}
