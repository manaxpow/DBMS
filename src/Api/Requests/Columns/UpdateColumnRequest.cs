public sealed record UpdateColumnRequest(
    string Name,
    string DataType,
    bool IsNullable);
