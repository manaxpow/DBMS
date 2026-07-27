public class Column
{
    public int Id { get; set; }
    public string Name { get; set; }
    public IDataType DataType { get; set; }
    public bool IsNullable { get; set; }

    public Column(string name, IDataType dataType)
    {
        Name = name;
        DataType = dataType;
    }

    public Column(string name, IDataType dataType, bool isNullable)
    {
        Name = name;
        DataType = dataType;
        IsNullable = isNullable;
    }

    // Legacy constructors for test suite compatibility
    public Column(string name, System.Type type)
    {
        Name = name;
        DataType = type == typeof(string) ? (IDataType)new VarcharType() : new IntegerType();
    }

    public Column(string name, System.Type type, bool isNullable)
    {
        Name = name;
        DataType = type == typeof(string) ? (IDataType)new VarcharType() : new IntegerType();
        IsNullable = isNullable;
    }

    public Column Create(string name, string type, bool isNullable) => throw new NotImplementedException();
    public bool ValidateValue(object value) => throw new NotImplementedException();
    public void Validate() => throw new NotImplementedException();
}
