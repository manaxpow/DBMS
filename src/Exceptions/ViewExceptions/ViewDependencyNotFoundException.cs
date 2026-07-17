using System;

public class ViewDependencyNotFoundException : Exception
{
    public ViewDependencyNotFoundException() : base() { }
    public ViewDependencyNotFoundException(string message) : base(message) { }
    public ViewDependencyNotFoundException(string message, Exception inner) : base(message, inner) { }
}
