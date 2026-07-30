using System;

public class PartitionRange
{
    public object Start { get; set; } = null!;

    public object End { get; set; } = null!;

    public bool IncludeStart { get; set; }

    public bool IncludeEnd { get; set; }

    public Partition Target { get; set; } = null!;

    public bool Contains(object key) => throw new NotImplementedException();

    public bool Overlaps(PartitionRange other) => throw new NotImplementedException();
}
