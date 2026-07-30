public class TableBuilder : ITableBuilder
{
    private string _name;
    private List<Column> _columns;
    private List<Constraint> _constraints;
    private List<Index> _indexes;
    private List<Partition> _partitions;

    public TableBuilder()
    {
        this._name = string.Empty;
        this._columns = new List<Column>();
        this._constraints = new List<Constraint>();
        this._indexes = new List<Index>();
        this._partitions = new List<Partition>();
    }

    public Table Build() => throw new NotImplementedException();

    public ITableBuilder SetName(string name) => throw new NotImplementedException();

    public ITableBuilder AddColumn(Column column) => throw new NotImplementedException();

    public ITableBuilder AddConstraint(Constraint constraint) => throw new NotImplementedException();

    public ITableBuilder AddIndex(Index index) => throw new NotImplementedException();

    public ITableBuilder AddPartition(Partition partition) => throw new NotImplementedException();
}
