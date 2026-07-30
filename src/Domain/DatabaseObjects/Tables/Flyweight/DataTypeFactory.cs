public static class DataTypeFactory
{
    private static readonly Dictionary<string, IDataType> cache = new(StringComparer.OrdinalIgnoreCase);

    public static IDataType Create(string name)
    {
        if (!cache.TryGetValue(name, out var type))
        {
            type = name.ToUpper() switch
            {
                "INT" => new IntegerType(),
                "VARCHAR" => new VarcharType(),
                "DATETIME" => new DateTimeType(),
                "BOOLEAN" => new BooleanType(),
                _ => throw new NotSupportedException($"Data type {name} not supported.")
            };
            cache[name] = type;
        }

        return type;
    }
}
