public class VarcharType : IDataType
{
    public string Name => "VARCHAR";

    public int Size => 255;

    public int MaxLength => 255;

    public void Validate(Column context, object value)
    {
    }
}
