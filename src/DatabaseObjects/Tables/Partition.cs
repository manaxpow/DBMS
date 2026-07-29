public class Partition
{
    private List<PartitionRange> _ranges;

    public Partition(string name)
    {
        this.Name = name;
        this._ranges = new List<PartitionRange>();
        this.Ranges = this._ranges.AsReadOnly();
    }

    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public IReadOnlyList<PartitionRange> Ranges { get; }

    public Partition RouteRow(Row row, string partitionKey) => throw new NotImplementedException();

    public void AddRange(PartitionRange range) => throw new NotImplementedException();

    public void RemoveRange(PartitionRange range) => throw new NotImplementedException();

    private PartitionRange FindMatchingRange(object key) => throw new NotImplementedException();

    private bool HasOverlappingRange(PartitionRange range) => throw new NotImplementedException();
}
