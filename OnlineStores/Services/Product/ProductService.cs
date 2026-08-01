using OnlineStores.Entities;
using OnlineStores.Repositories;

namespace OnlineStores.Services;

public class ProductService(IProductRepository productRepository) : IProductService
{
    private readonly IProductRepository _productRepository = productRepository;

    public async Task<PagedResponse<ProductResponse>> GetProductsAsync(GetProductsQuery query, CancellationToken cancellationToken = default)
    {
        var products = await _productRepository.GetAllAsync(query, cancellationToken);
        if (products is not null) return new PagedResponse<ProductResponse>(products.Select(MapToProductResponse), products.Count(), query.Page, query.PageSize);
        return new PagedResponse<ProductResponse>(Array.Empty<ProductResponse>(), 0, query.Page, query.PageSize);
    }

    public async Task<ProductResponse?> GetProductByIdAsync(Guid productId, bool includeImages = false, bool includeVariants = false, bool includeStatistics = false, CancellationToken cancellationToken = default)
    {
        var product = await _productRepository.GetByIdAsync(productId, cancellationToken);
        if (product is null) return null;
        return MapToProductResponse(product);
    }

    public async Task<ProductResponse> CreateProductAsync(CreateProductRequest request, bool publishImmediately = false, CancellationToken cancellationToken = default)
    {
        var product = Product.Create(
            storeId: Guid.NewGuid(), // Mock store ID
            name: request.Name,
            description: request.Description,
            price: request.Price,
            stockQuantity: request.StockQuantity,
            type: request.Type,
            categoryId: request.CategoryId,
            status: publishImmediately ? ProductStatus.Published : ProductStatus.Draft
        );

        await _productRepository.AddAsync(product, cancellationToken);
        return MapToProductResponse(product);
    }

    public async Task<ProductResponse> UpdateProductAsync(Guid productId, UpdateProductRequest request, CancellationToken cancellationToken = default)
    {
        var product = await _productRepository.GetByIdAsync(productId, cancellationToken);
        if (product is null) throw new Exception("Product not found");

        product.Update(request.Name, request.Description, request.Price, request.StockQuantity, request.Type, request.CategoryId, request.Status);

        await _productRepository.UpdateAsync(product, cancellationToken);
        return MapToProductResponse(product);
    }

    public async Task DeleteProductAsync(Guid productId, bool force = false, bool deleteAssets = false, CancellationToken cancellationToken = default)
    {
        var product = await _productRepository.GetByIdAsync(productId, cancellationToken);
        if (product is null) throw new Exception("Product not found");
        await _productRepository.DeleteAsync(product, cancellationToken);
    }

    public async Task<ProductResponse> UploadProductImageAsync(Guid productId, IFormFile image, bool setAsPrimary = false, int? position = null, CancellationToken cancellationToken = default)
    {
        var product = await _productRepository.GetByIdAsync(productId, cancellationToken);
        if (product is null) throw new Exception("Product not found");
        // Mock image upload handling
        return MapToProductResponse(product);
    }

    private ProductResponse MapToProductResponse(Product product)
    {
        return new ProductResponse(
            product.Id,
            product.StoreId,
            product.Name,
            product.Description,
            product.Price,
            product.StockQuantity,
            product.Status,
            product.Type,
            product.CategoryId,
            product.CreatedAt,
            product.UpdatedAt
        );
    }
}
