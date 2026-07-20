using System;

public class PartitionRangeNotFoundException : Exception
{
    public PartitionRangeNotFoundException() : base() { }
    public PartitionRangeNotFoundException(string message) : base(message) { }
    public PartitionRangeNotFoundException(string message, Exception inner) : base(message, inner) { }
}
