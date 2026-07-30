public sealed record CreateColumnRequest(
    string Name,
    string DataType,
    bool IsNullable);

public sealed record GetColumnRequest(
    string Name);

public sealed record UpdateColumnRequest(
    string Name,
    string DataType,
    bool IsNullable);

public sealed record DeleteColumnRequest(
    string Name);

public sealed record GetColumnsRequest();
