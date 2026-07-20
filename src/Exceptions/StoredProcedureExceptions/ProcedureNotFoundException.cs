using System;

public class ProcedureNotFoundException : Exception
{
    public ProcedureNotFoundException() : base() { }
    public ProcedureNotFoundException(string message) : base(message) { }
    public ProcedureNotFoundException(string message, Exception inner) : base(message, inner) { }
}
