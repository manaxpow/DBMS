public record ProductResponse(
    Guid Id,
    Guid StoreId,
    string Name,
    string? Description,
    decimal Price,
    int StockQuantity,
    ProductStatus Status,
    ProductType? Type,
    Guid? CategoryId,
    DateTime CreatedAt,
    DateTime? UpdatedAt
);
