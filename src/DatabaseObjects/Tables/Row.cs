using System;
using System.Collections.Generic;

public class Row
{
    private List<object> _values;

    public Row(Table table, List<object> values)
    {
        this.Table = table;
        this._values = values;
        this.Values = this._values.AsReadOnly();
    }

    public int Id { get; set; }

    public Table Table { get; set; } = null!;

    public IReadOnlyList<object> Values { get; }

    public object GetValue(string columnName) => throw new NotImplementedException();

    public void SetValue(string columnName, object value) => throw new NotImplementedException();

    internal void RemoveValueAt(int columnIndex) => throw new NotImplementedException();
}
