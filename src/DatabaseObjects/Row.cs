using System;

public class Row
{
    public object[] Values { get; set; }
    public Row()
    {
        Values = new object[0];
    }
    public object GetValue(string columnName)
    {
        throw new NotImplementedException();
    }

    public void SetValue(string columnName, object value)
    {
        throw new NotImplementedException();
    }
}
