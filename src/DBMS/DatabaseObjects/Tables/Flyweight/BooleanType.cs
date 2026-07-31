public class BooleanType : IDataType
{
    public string Name => "BOOLEAN";

    public int Size => 1;

    public void Validate(Column context, object value)
    {
    }
}
