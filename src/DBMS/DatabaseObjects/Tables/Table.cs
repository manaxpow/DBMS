public class Table : ISchemaObject, ICatalogObject
{
    private List<Column> _columns;
    private List<Row> _rows;
    private List<Constraint> _constraints;
    private List<Index> _indexes;
    private List<Partition> _partitions;
    private int _nextRowId = 1;
    private int _nextColumnId = 1;

    public Table(string name)
    {
        this.Name = name;
        this._columns = new List<Column>();
        this._rows = new List<Row>();
        this._constraints = new List<Constraint>();
        this._indexes = new List<Index>();
        this._partitions = new List<Partition>();
        this.Columns = this._columns.AsReadOnly();
        this.Rows = this._rows.AsReadOnly();
        this.Constraints = this._constraints.AsReadOnly();
        this.Indexes = this._indexes.AsReadOnly();
        this.Partitions = this._partitions.AsReadOnly();
    }

    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public SchemaObjectType ObjectType => SchemaObjectType.Table;

    public IReadOnlyList<Column> Columns { get; }

    public IReadOnlyList<Row> Rows { get; }

    public IReadOnlyList<Constraint> Constraints { get; }

    public IReadOnlyList<Index> Indexes { get; }

    public IReadOnlyList<Partition> Partitions { get; }

    public void AddColumn(Column column)
    {
        column.Id = _nextColumnId++;
        _columns.Add(column);
    }

    public void DropColumn(string columnName)
    {
        _columns.RemoveAll(c => c.Name == columnName);
    }

    public void AlterColumn(string columnName, Column newColumn) => throw new NotImplementedException();

    public void AddConstraint(Constraint constraint) => throw new NotImplementedException();

    public void DropConstraint(string constraintName) => throw new NotImplementedException();

    public void InsertRow(Row row)
    {
        row.Id = _nextRowId++;
        row.Table = this;
        _rows.Add(row);
    }

    public void UpdateRow(int id, Row newRow)
    {
        var index = _rows.FindIndex(r => r.Id == id);
        if (index >= 0)
        {
            newRow.Id = id;
            newRow.Table = this;
            _rows[index] = newRow;
        }
    }

    public bool DeleteRow(int id)
    {
        return _rows.RemoveAll(r => r.Id == id) > 0;
    }

    public void Drop() => throw new NotImplementedException();

    public bool ContainsColumn(string columnName)
    {
        return this._columns.Any(c => c.Name == columnName);
    }

    public bool ContainsRow(Row row)
    {
        return this._rows.Contains(row);
    }

    public Column? GetColumn(string columnName)
    {
        return this._columns.FirstOrDefault(c => c.Name == columnName);
    }

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
