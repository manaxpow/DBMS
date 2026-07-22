public class Partition
{
    public int Id { get; set; }

    public string Name { get; set; }
    public IReadOnlyList<PartitionRange> Ranges { get; }

    private List<PartitionRange> _ranges;

    public Partition(string name)
    {
        Name = name;
        _ranges = new List<PartitionRange>();
        Ranges = _ranges.AsReadOnly();
    }

    public Partition RouteRow(Row row, string partitionKey) => throw new NotImplementedException();
    public void AddRange(PartitionRange range) => throw new NotImplementedException();
    public void RemoveRange(PartitionRange range) => throw new NotImplementedException();

    private PartitionRange FindMatchingRange(object key) => throw new NotImplementedException();
    private bool HasOverlappingRange(PartitionRange range) => throw new NotImplementedException();
}
