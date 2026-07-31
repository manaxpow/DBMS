using System.Collections.Generic;

public record TableDetailResponse(
    int Id, 
    string Name,
    IReadOnlyList<ColumnResponse>? Columns = null,
    object? Constraints = null,
    object? Indexes = null,
    object? Partitions = null
);
