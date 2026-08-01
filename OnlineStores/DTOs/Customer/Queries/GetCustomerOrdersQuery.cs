public record GetCustomerOrdersQuery(
    string? Status,
    string? PaymentStatus,
    DateTime? CreatedFrom,
    DateTime? CreatedTo,
    string? Sort,
    int Page = 1,
    int PageSize = 20
);
