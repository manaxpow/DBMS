public record CreateProductRequest(
    string Name,
    string? Description,
    decimal Price,
    int StockQuantity,
    ProductType? Type,
    Guid? CategoryId
);
