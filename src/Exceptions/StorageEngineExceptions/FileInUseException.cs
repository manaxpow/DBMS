using System;

public class FileInUseException : Exception
{
    public FileInUseException() : base() { }
    public FileInUseException(string message) : base(message) { }
    public FileInUseException(string message, Exception inner) : base(message, inner) { }
}
