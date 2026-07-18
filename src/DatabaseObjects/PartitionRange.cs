using System;

public class PartitionRange
{
    public object Start { get; set; }
    public object End { get; set; }
    public bool IncludeStart { get; set; }
    public bool IncludeEnd { get; set; }
    public Partition Target { get; set; }
    
    public bool Contains(object key) => throw new NotImplementedException();
    public bool Overlaps(PartitionRange other) => throw new NotImplementedException();
}
