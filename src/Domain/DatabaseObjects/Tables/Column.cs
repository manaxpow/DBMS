public class Column(string name, IDataType dataType, bool isNullable = false)
{
    public int Id { get; set; }

    public string Name { get; set; } = name;

    public IDataType DataType { get; set; } = dataType;

    public bool IsNullable { get; set; } = isNullable;

    public Column Create(string name, string type, bool isNullable) => throw new NotImplementedException();

    public bool ValidateValue(object value) => throw new NotImplementedException();

    public void Validate() => throw new NotImplementedException();
}
