using System;
using System.Collections.Generic;

public class Row
{
    public Table Table { get; set; }
    public IReadOnlyList<object> Values { get; }
    
    private List<object> _values;

    public Row(Table table, List<object> values)
    {
        Table = table;
        _values = values;
        Values = _values.AsReadOnly();
    }

    public object GetValue(string columnName) => throw new NotImplementedException();
    public void SetValue(string columnName, object value) => throw new NotImplementedException();
    internal void RemoveValueAt(int columnIndex) => throw new NotImplementedException();
}
