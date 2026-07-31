public record ColumnDetailResponse(int Id, string Name, IDataType DataType, bool IsNullable, object? Dependencies);
