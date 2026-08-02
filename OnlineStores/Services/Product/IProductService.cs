

public interface IProductService
{
    Task<PagedResponse<ProductResponse>> GetProductsAsync(GetProductsQuery query, CancellationToken cancellationToken = default);
    Task<ProductResponse?> GetProductByIdAsync(Guid productId, bool includeImages = false, bool includeVariants = false, bool includeStatistics = false, CancellationToken cancellationToken = default);
    Task<ProductResponse> CreateProductAsync(CreateProductRequest request, bool publishImmediately = false, CancellationToken cancellationToken = default);
    Task<ProductResponse> UpdateProductAsync(Guid productId, UpdateProductRequest request, CancellationToken cancellationToken = default);
    Task DeleteProductAsync(Guid productId, bool force = false, bool deleteAssets = false, CancellationToken cancellationToken = default);
    Task<ProductResponse> UploadProductImageAsync(Guid productId, Microsoft.AspNetCore.Http.IFormFile image, bool setAsPrimary = false, int? position = null, CancellationToken cancellationToken = default);

}
