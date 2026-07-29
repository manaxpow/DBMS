using System;

public class PartitionRouteNotFoundException : Exception
{
    public PartitionRouteNotFoundException()
        : base()
    {
    }

    public PartitionRouteNotFoundException(string message)
        : base(message)
    {
    }

    public PartitionRouteNotFoundException(string message, Exception inner)
        : base(message, inner)
    {
    }
}
