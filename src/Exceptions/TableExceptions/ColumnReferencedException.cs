using System;

public class ColumnReferencedException : Exception
{
    public ColumnReferencedException() : base() { }
    public ColumnReferencedException(string message) : base(message) { }
    public ColumnReferencedException(string message, Exception inner) : base(message, inner) { }
}
