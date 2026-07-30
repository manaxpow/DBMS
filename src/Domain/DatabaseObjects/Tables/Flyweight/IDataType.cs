public interface IDataType
{
    string Name { get; }

    int Size { get; }

    void Validate(Column context, object value);
}
