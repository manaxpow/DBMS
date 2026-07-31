public class IntegerType : IDataType
{
    public string Name => "INT";

    public int Size => 4;

    public void Validate(Column context, object value)
    {
        if (value == null)
        {
            if (!context.IsNullable)
            {
                throw new InvalidOperationException($"{context.Name} cannot be null.");
            }

            return;
        }

        if (value is not int)
        {
            throw new InvalidOperationException($"{context.Name} must contain an integer.");
        }
    }
}
