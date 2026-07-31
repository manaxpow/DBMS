public sealed record AlterColumnRequest(
    string Name,
    string DataType,
    bool IsNullable);
