using System;

public class ProcedureDisabledException : Exception
{
    public ProcedureDisabledException() : base() { }
    public ProcedureDisabledException(string message) : base(message) { }
    public ProcedureDisabledException(string message, Exception inner) : base(message, inner) { }
}
