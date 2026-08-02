public record ExportCustomersQuery(
    string? Search,
    CustomerStatus? Status,
    string? Category,
    DateTime? CreatedFrom,
    DateTime? CreatedTo,
    string? Format = "csv"
);
