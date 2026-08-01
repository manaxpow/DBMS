public record UpdateProductRequest(
    string Name,
    string? Description,
    decimal Price,
    int StockQuantity,
    ProductType? Type,
    Guid? CategoryId,
    ProductStatus Status
);
