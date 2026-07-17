using System;

public class Table
{
    public string Name { get; set; }
    public List<Row> Rows { get; }
    public List<Column> Columns { get; }
    public Table(string name)
    {
        Name = name;
        Rows = new List<Row>();
        Columns = new List<Column>();
    }

    public void AddColumn(Column column)
    {
        throw new NotImplementedException();
    }

    public void DropColumn(string columnName)
    {
        throw new NotImplementedException();
    }

    public void AlterColumn(string columnName, Column newColumn)
    {
        throw new NotImplementedException();
    }

    public void InsertRow(Row row)
    {
        throw new NotImplementedException();
    }

    public void DeleteRow(Row row)
    {
        throw new NotImplementedException();
    }

    public bool ContainsColumn(string columnName)
    {
        throw new NotImplementedException();
    }

    public bool ContainsRow(Row row)
    {
        throw new NotImplementedException();
    }

    public Column GetColumn(string columnName)
    {
        throw new NotImplementedException();
    }

    public int GetColumnIndex(Column column)
    {
        throw new NotImplementedException();
    }

    public Index GetPrimaryIndex()
    {
        throw new NotImplementedException();
    }

    public Index GetForeignKeyIndex()
    {
        throw new NotImplementedException();
    }
    private bool ValidateValueCount(Row row)
    {
        throw new NotImplementedException();
    }
}
