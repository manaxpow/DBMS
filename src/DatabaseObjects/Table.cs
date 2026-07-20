using System;
using System.Collections.Generic;

public class Table
{
    public string Name { get; set; }
    
    public IReadOnlyList<Column> Columns { get; }
    public IReadOnlyList<Row> Rows { get; }
    public IReadOnlyList<Constraint> Constraints { get; }
    public IReadOnlyList<Index> Indexes { get; }
    public IReadOnlyList<Partition> Partitions { get; }

    private List<Column> _columns;
    private List<Row> _rows;
    private List<Constraint> _constraints;
    private List<Index> _indexes;
    private List<Partition> _partitions;

    public Table(string name)
    {
        Name = name;
        _columns = new List<Column>();
        _rows = new List<Row>();
        _constraints = new List<Constraint>();
        _indexes = new List<Index>();
        _partitions = new List<Partition>();
        Columns = _columns.AsReadOnly();
        Rows = _rows.AsReadOnly();
        Constraints = _constraints.AsReadOnly();
        Indexes = _indexes.AsReadOnly();
        Partitions = _partitions.AsReadOnly();
    }

    public void AddColumn(Column column) => throw new NotImplementedException();
    public void DropColumn(string columnName) => throw new NotImplementedException();
    public void AlterColumn(string columnName, Column newColumn) => throw new NotImplementedException();
    public void AddConstraint(Constraint constraint) => throw new NotImplementedException();
    public void DropConstraint(string constraintName) => throw new NotImplementedException();
    public void InsertRow(Row row) => throw new NotImplementedException();
    public bool DeleteRow(Row row) => throw new NotImplementedException();
    public bool ContainsColumn(string columnName) => throw new NotImplementedException();
    public bool ContainsRow(Row row) => throw new NotImplementedException();
    public Column GetColumn(string columnName) => throw new NotImplementedException();
    public int GetColumnIndex(Column column) => throw new NotImplementedException();
    public int GetColumnIndex(string columnName) => throw new NotImplementedException();
    public Index GetPrimaryIndex() => throw new NotImplementedException();
    public Index GetForeignKeyIndex() => throw new NotImplementedException();
    
    private bool ValidateValueCount(Row row) => throw new NotImplementedException();
    private bool ValidateRowValues(Row row) => throw new NotImplementedException();
    private bool IsColumnReferencedByConstraint(string columnName) => throw new NotImplementedException();
    private void RemoveColumnValues(int columnIndex) => throw new NotImplementedException();
}


