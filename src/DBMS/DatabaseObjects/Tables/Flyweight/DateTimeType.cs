public class DateTimeType : IDataType
{
    public string Name => "DATETIME";

    public int Size => 8;

    public void Validate(Column context, object value)
    {
    }
}
