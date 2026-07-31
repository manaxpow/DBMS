using System;

public class ProcedureParameterException : Exception
{
    public ProcedureParameterException()
        : base()
    {
    }

    public ProcedureParameterException(string message)
        : base(message)
    {
    }

    public ProcedureParameterException(string message, Exception inner)
        : base(message, inner)
    {
    }
}
