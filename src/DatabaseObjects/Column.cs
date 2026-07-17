using System;

public class Column
{
    public string Name { get; set; }
    public string Type { get; set; }

    public static Column Create()
    {
        throw new NotImplementedException();
    }

    public bool ValidateValue(object value)
    {
        throw new NotImplementedException();
    }
}
