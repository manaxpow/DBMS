




using Microsoft.Extensions.Caching.Memory;

public class ProductService(IProductRepository productRepository, IMemoryCache memoryCache) : IProductService
{
    private readonly IProductRepository _productRepository = productRepository;
    private readonly IMemoryCache _memoryCache = memoryCache;

    public async Task<PagedResponse<ProductResponse>> GetProductsAsync(GetProductsQuery query, CancellationToken cancellationToken = default)
    {
        var products = await _productRepository.GetAllAsync(query, cancellationToken);
        if (products is not null) return new PagedResponse<ProductResponse>(products.Select(MapToProductResponse), products.Count(), query.Page, query.PageSize);
        return new PagedResponse<ProductResponse>(Array.Empty<ProductResponse>(), 0, query.Page, query.PageSize);
    }

    public async Task<ProductResponse?> GetProductByIdAsync(Guid productId, bool includeImages = false, bool includeVariants = false, bool includeStatistics = false, CancellationToken cancellationToken = default)
    {

        // Check cache first
        var cacheKey = CacheKeys.Product.ById(productId);
        if (_memoryCache.TryGetValue(cacheKey, out ProductResponse? cachedProduct))
        {
            return cachedProduct;
        }

        var product = await _productRepository.GetByIdAsync(productId, cancellationToken);
        if (product is null) return null;

        var productResponse = MapToProductResponse(product);

        // Cache
        _memoryCache.Set(cacheKey, productResponse, TimeSpan.FromMinutes(10));
        return productResponse;
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

        // Invalidate cache
        string cacheKey = CacheKeys.Product.ById(productId);
        _memoryCache.Remove(cacheKey);
        return MapToProductResponse(product);
    }

    public async Task DeleteProductAsync(Guid productId, bool force = false, bool deleteAssets = false, CancellationToken cancellationToken = default)
    {
        var product = await _productRepository.GetByIdAsync(productId, cancellationToken);
        if (product is null) throw new NotFoundException("Product not found");
        await _productRepository.DeleteAsync(product, cancellationToken);

        // Invalidate cache
        string cacheKey = CacheKeys.Product.ById(productId);
        _memoryCache.Remove(cacheKey);
    }

    public async Task<ProductResponse> UploadProductImageAsync(Guid productId, IFormFile image, bool setAsPrimary = false, int? position = null, CancellationToken cancellationToken = default)
    {
        var product = await _productRepository.GetByIdAsync(productId, cancellationToken);
        if (product is null) throw new NotFoundException("Product not found");
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
