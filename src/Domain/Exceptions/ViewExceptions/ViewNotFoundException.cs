using System;

public class ViewNotFoundException : Exception
{
    public ViewNotFoundException()
        : base()
    {
    }

    public ViewNotFoundException(string message)
        : base(message)
    {
    }

    public ViewNotFoundException(string message, Exception inner)
        : base(message, inner)
    {
    }
}
