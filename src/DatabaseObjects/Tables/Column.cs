public class Column(string name, IDataType dataType, bool isNullable = false)
{
    // Legacy constructors for test suite compatibility
    public Column(string name, System.Type type)
        : this(name, type == typeof(string) ? (IDataType)new VarcharType() : new IntegerType(), false)
    {
    }

    public Column(string name, System.Type type, bool isNullable)
        : this(name, type == typeof(string) ? (IDataType)new VarcharType() : new IntegerType(), isNullable)
    {
    }

    public int Id { get; set; }

    public string Name { get; set; } = name;

    public IDataType DataType { get; set; } = dataType;

    public bool IsNullable { get; set; } = isNullable;

    public Column Create(string name, string type, bool isNullable) => throw new NotImplementedException();

    public bool ValidateValue(object value) => throw new NotImplementedException();

    public void Validate() => throw new NotImplementedException();
}
