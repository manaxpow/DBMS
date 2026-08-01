public record GetProductsQuery(
    string? Search,
    ProductStatus? Status,
    ProductType? Type,
    Guid? CategoryId,
    decimal? MinPrice,
    decimal? MaxPrice,
    string? StockStatus,
    DateTime? CreatedFrom,
    DateTime? CreatedTo,
    string? Sort,
    int Page = 1,
    int PageSize = 20
);
