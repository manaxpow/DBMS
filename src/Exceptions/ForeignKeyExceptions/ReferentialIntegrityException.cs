using System;

public class ReferentialIntegrityException : Exception
{
    public ReferentialIntegrityException()
        : base()
    {
    }

    public ReferentialIntegrityException(string message)
        : base(message)
    {
    }

    public ReferentialIntegrityException(string message, Exception inner)
        : base(message, inner)
    {
    }
}
