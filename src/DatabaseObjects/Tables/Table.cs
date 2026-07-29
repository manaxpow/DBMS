public class Table : ISchemaObject, ICatalogObject
{
    private List<Column> columns;
    private List<Row> rows;
    private List<Constraint> constraints;
    private List<Index> indexes;
    private List<Partition> partitions;

    public Table(string name)
    {
        this.Name = name;
        this.columns = new List<Column>();
        this.rows = new List<Row>();
        this.constraints = new List<Constraint>();
        this.indexes = new List<Index>();
        this.partitions = new List<Partition>();
        this.Columns = this.columns.AsReadOnly();
        this.Rows = this.rows.AsReadOnly();
        this.Constraints = this.constraints.AsReadOnly();
        this.Indexes = this.indexes.AsReadOnly();
        this.Partitions = this.partitions.AsReadOnly();
    }

    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public SchemaObjectType ObjectType => SchemaObjectType.Table;

    public IReadOnlyList<Column> Columns { get; }

    public IReadOnlyList<Row> Rows { get; }

    public IReadOnlyList<Constraint> Constraints { get; }

    public IReadOnlyList<Index> Indexes { get; }

    public IReadOnlyList<Partition> Partitions { get; }

    public void AddColumn(Column column) => throw new NotImplementedException();

    public void DropColumn(string columnName) => throw new NotImplementedException();

    public void AlterColumn(string columnName, Column newColumn) => throw new NotImplementedException();

    public void AddConstraint(Constraint constraint) => throw new NotImplementedException();

    public void DropConstraint(string constraintName) => throw new NotImplementedException();

    public void InsertRow(Row row) => throw new NotImplementedException();

    public void UpdateRow(Row oldRow, Row newRow) => throw new NotImplementedException();

    public bool DeleteRow(Row row) => throw new NotImplementedException();

    public void Drop() => throw new NotImplementedException();

    public bool ContainsColumn(string columnName) => throw new NotImplementedException();

    public bool ContainsRow(Row row) => throw new NotImplementedException();

    public Column GetColumn(string columnName) => throw new NotImplementedException();

    public int GetColumnIndex(Column column) => throw new NotImplementedException();

    public int GetColumnIndex(string columnName) => throw new NotImplementedException();

    public Index GetPrimaryIndex() => throw new NotImplementedException();

    public Index GetForeignKeyIndex() => throw new NotImplementedException();

    public void Accept(ISchemaVisitor visitor)
    {
        throw new NotImplementedException();
    }

    public ISchemaObject Clone() => throw new NotImplementedException();

    private bool ValidateValueCount(Row row) => throw new NotImplementedException();

    private bool ValidateRowValues(Row row) => throw new NotImplementedException();

    private bool IsColumnReferencedByConstraint(string columnName) => throw new NotImplementedException();

    private void RemoveColumnValues(int columnIndex) => throw new NotImplementedException();
}
