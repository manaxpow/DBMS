public class TableBuilder : ITableBuilder
{
    private string name;
    private List<Column> columns;
    private List<Constraint> constraints;
    private List<Index> indexes;
    private List<Partition> partitions;

    public TableBuilder()
    {
        this.name = string.Empty;
        this.columns = new List<Column>();
        this.constraints = new List<Constraint>();
        this.indexes = new List<Index>();
        this.partitions = new List<Partition>();
    }

    public Table Build() => throw new NotImplementedException();

    public ITableBuilder SetName(string name) => throw new NotImplementedException();

    public ITableBuilder AddColumn(Column column) => throw new NotImplementedException();

    public ITableBuilder AddConstraint(Constraint constraint) => throw new NotImplementedException();

    public ITableBuilder AddIndex(Index index) => throw new NotImplementedException();

    public ITableBuilder AddPartition(Partition partition) => throw new NotImplementedException();
}
