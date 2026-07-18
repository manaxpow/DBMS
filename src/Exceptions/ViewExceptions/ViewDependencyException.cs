using System;

public class ViewDependencyException : Exception
{
    public ViewDependencyException() : base() { }
    public ViewDependencyException(string message) : base(message) { }
    public ViewDependencyException(string message, Exception inner) : base(message, inner) { }
}
