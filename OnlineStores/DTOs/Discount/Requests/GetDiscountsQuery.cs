public record GetDiscountsQuery(
    string? Search,
    DiscountStatus? Status,
    DiscountType? Type,
    string? ApplicableTo,
    DateTime? StartsFrom,
    DateTime? StartsTo,
    DateTime? ExpiresFrom,
    DateTime? ExpiresTo,
    string? Sort,
    int Page = 1,
    int PageSize = 20
);
