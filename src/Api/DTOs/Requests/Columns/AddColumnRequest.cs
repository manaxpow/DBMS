public sealed record AddColumnRequest(
    string Name,
    string DataType,
    bool IsNullable);
