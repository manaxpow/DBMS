namespace OnlineStores.Entities;

public class Product
{
    public Guid Id { get; private set; }
    public Guid StoreId { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public decimal Price { get; private set; }
    public int StockQuantity { get; private set; }
    public ProductStatus Status { get; private set; } = ProductStatus.Draft;
    public ProductType? Type { get; private set; }
    public Guid? CategoryId { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    private Product() { }

    public static Product Create(Guid storeId, string name, string? description, decimal price, int stockQuantity, ProductType? type, Guid? categoryId, ProductStatus status = ProductStatus.Draft)
    {
        return new Product
        {
            Id = Guid.NewGuid(),
            StoreId = storeId,
            Name = name,
            Description = description,
            Price = price,
            StockQuantity = stockQuantity,
            Type = type,
            CategoryId = categoryId,
            Status = status,
            CreatedAt = DateTime.UtcNow
        };
    }

    public void Update(string name, string? description, decimal price, int stockQuantity, ProductType? type, Guid? categoryId, ProductStatus status)
    {
        Name = name;
        Description = description;
        Price = price;
        StockQuantity = stockQuantity;
        Type = type;
        CategoryId = categoryId;
        Status = status;
        UpdatedAt = DateTime.UtcNow;
    }
}
