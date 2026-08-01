using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OnlineStores.Services;

[ApiController]
[Route("api/v1/products")]
[Authorize]
public class ProductsController(IProductService productService) : ControllerBase
{
    private readonly IProductService _productService = productService;

    [AllowAnonymous]
    [HttpGet]
    public async Task<ActionResult<PagedResponse<ProductResponse>>> GetProducts([FromQuery] GetProductsQuery query, CancellationToken cancellationToken)
    {
        var products = await _productService.GetProductsAsync(query, cancellationToken);
        return Ok(products);
    }

    [AllowAnonymous]
    [HttpGet("{productId}")]
    public async Task<ActionResult<ProductResponse>> GetProductById(
        Guid productId,
        [FromQuery] GetProductByIdQuery query,
        CancellationToken cancellationToken = default)
    {
        var product = await _productService.GetProductByIdAsync(productId, query.IncludeImages ?? false, query.IncludeVariants ?? false, query.IncludeStatistics ?? false, cancellationToken);
        if (product is null)
        {
            return NotFound();
        }

        return Ok(product);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<ActionResult<ProductResponse>> CreateProduct(
        [FromBody] CreateProductRequest request,
        [FromQuery] CreateProductQuery query,
        CancellationToken cancellationToken = default)
    {
        var product = await _productService.CreateProductAsync(request, query.PublishImmediately ?? false, cancellationToken);
        return CreatedAtAction(nameof(GetProductById), new { productId = product.Id }, product);
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{productId}")]
    public async Task<ActionResult<ProductResponse>> UpdateProduct(
        Guid productId,
        [FromBody] UpdateProductRequest request,
        CancellationToken cancellationToken = default)
    {
        var product = await _productService.UpdateProductAsync(productId, request, cancellationToken);
        return Ok(product);
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{productId}")]
    public async Task<ActionResult> DeleteProduct(
        Guid productId,
        [FromQuery] DeleteProductQuery query,
        CancellationToken cancellationToken = default)
    {
        await _productService.DeleteProductAsync(productId, query.Force ?? false, query.DeleteAssets ?? false, cancellationToken);
        return NoContent();
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("{productId}/images")]
    public async Task<ActionResult<ProductResponse>> UploadProductImage(
        Guid productId,
        [FromForm] UploadProductImageRequest request,
        [FromQuery] UploadProductImageQuery query,
        CancellationToken cancellationToken = default)
    {
        var product = await _productService.UploadProductImageAsync(productId, request.Image, query.SetAsPrimary ?? false, query.Position, cancellationToken);
        return Ok(product);
    }
}
