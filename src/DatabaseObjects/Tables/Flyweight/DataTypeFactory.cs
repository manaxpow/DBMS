public class DataTypeFactory
{
    private Dictionary<string, IDataType> cache = new ();

    public IDataType GetDataType(string name)
    {
        name = name.ToUpper();
        if (!this.cache.TryGetValue(name, out var type))
        {
            type = name switch
            {
                "INT" => new IntegerType(),
                "VARCHAR" => new VarcharType(),
                "DATETIME" => new DateTimeType(),
                "BOOLEAN" => new BooleanType(),
                _ => throw new NotSupportedException($"Data type {name} not supported.")
            };
            this.cache[name] = type;
        }

        return type;
    }
}
