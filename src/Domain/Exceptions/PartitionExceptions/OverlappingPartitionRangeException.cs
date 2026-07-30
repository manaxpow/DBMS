using System;

public class OverlappingPartitionRangeException : Exception
{
    public OverlappingPartitionRangeException()
        : base()
    {
    }

    public OverlappingPartitionRangeException(string message)
        : base(message)
    {
    }

    public OverlappingPartitionRangeException(string message, Exception inner)
        : base(message, inner)
    {
    }
}
