using System;

public class NoAvailableFrameException : Exception
{
    public NoAvailableFrameException()
        : base()
    {
    }

    public NoAvailableFrameException(string message)
        : base(message)
    {
    }

    public NoAvailableFrameException(string message, Exception inner)
        : base(message, inner)
    {
    }
}
