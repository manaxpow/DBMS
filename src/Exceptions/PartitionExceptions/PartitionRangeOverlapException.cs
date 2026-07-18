using System;

public class PartitionRangeOverlapException : Exception
{
    public PartitionRangeOverlapException() : base() { }
    public PartitionRangeOverlapException(string message) : base(message) { }
    public PartitionRangeOverlapException(string message, Exception inner) : base(message, inner) { }
}
