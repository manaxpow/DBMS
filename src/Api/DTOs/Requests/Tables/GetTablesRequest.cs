public record GetTablesRequest(
    string? Name,
    bool? HasRows,
    bool? Partitioned,
    string? Sort,
    int? Page,
    int? PageSize
);
