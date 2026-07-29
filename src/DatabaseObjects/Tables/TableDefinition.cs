public sealed class TableDefinition
{
    public string Name { get; set; } = string.Empty;

    public List<Column> Columns { get; set; } = new List<Column>();

    public List<Constraint> Constraints { get; set; } = new List<Constraint>();

    public List<Index> Indexes { get; set; } = new List<Index>();

    public List<Partition> Partitions { get; set; } = new List<Partition>();
}
