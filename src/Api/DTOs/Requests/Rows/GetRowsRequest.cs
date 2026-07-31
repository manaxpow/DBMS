public record GetRowsRequest(
    string? Filter,
    string? Columns,
    string? Sort,
    bool? IncludeTotal,
    string? TransactionId,
    int? Page,
    int? PageSize
);
