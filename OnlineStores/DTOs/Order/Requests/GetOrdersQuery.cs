

public record GetOrdersQuery(
    string? Search,
    Guid? CustomerId,
    OrderStatus? Status,
    string? PaymentStatus,
    string? FulfillmentStatus,
    DateTime? CreatedFrom,
    DateTime? CreatedTo,
    decimal? MinTotal,
    decimal? MaxTotal,
    string? Sort,
    int Page = 1,
    int PageSize = 20
);
