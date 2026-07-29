using System;

public class MissingProcedureParameterException : Exception
{
    public MissingProcedureParameterException()
        : base()
    {
    }

    public MissingProcedureParameterException(string message)
        : base(message)
    {
    }

    public MissingProcedureParameterException(string message, Exception inner)
        : base(message, inner)
    {
    }
}
