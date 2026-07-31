public sealed record CreateColumnRequest(
    string Name,
    string DataType,
    bool IsNullable);
