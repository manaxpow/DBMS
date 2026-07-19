using System;

public class Column
{
    public string Name { get; set; }
    public Type DataType { get; set; }
    public bool IsNullable { get; set; }

    public Column(string name, Type dataType)
    {
        Name = name;
        DataType = dataType;
    }

    public Column(string name, Type dataType, bool isNullable)
    {
        Name = name;
        DataType = dataType;
        IsNullable = isNullable;
    }

    public Column Create(string name, string type, bool isNullable) => throw new NotImplementedException();
    public bool ValidateValue(object value) => throw new NotImplementedException();
    private Type ResolveDataType(string type) => throw new NotImplementedException();
}
