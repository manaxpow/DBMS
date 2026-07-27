public class DataTypeFactory
{
    private Dictionary<string, IDataType> _cache = new();

    public IDataType GetDataType(string name)
    {
        name = name.ToUpper();
        if (!_cache.TryGetValue(name, out var type))
        {
            type = name switch
            {
                "INT" => new IntegerType(),
                "VARCHAR" => new VarcharType(),
                "DATETIME" => new DateTimeType(),
                "BOOLEAN" => new BooleanType(),
                _ => throw new NotSupportedException($"Data type {name} not supported.")
            };
            _cache[name] = type;
        }
        return type;
    }
}
