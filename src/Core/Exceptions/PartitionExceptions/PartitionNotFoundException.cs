using System;

public class PartitionNotFoundException : Exception
{
    public PartitionNotFoundException()
        : base()
    {
    }

    public PartitionNotFoundException(string message)
        : base(message)
    {
    }

    public PartitionNotFoundException(string message, Exception inner)
        : base(message, inner)
    {
    }
}
