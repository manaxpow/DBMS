public class ProductRepository : IProductRepository
{
    private readonly List<Product> _products = new List<Product>();

    public Task AddAsync(Product product, CancellationToken cancellationToken = default)
    {
        _products.Add(product);
        return Task.CompletedTask;
    }

    public Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var product = _products.FirstOrDefault(p => p.Id == id);
        return Task.FromResult(product);
    }

    public Task UpdateAsync(Product product, CancellationToken cancellationToken = default)
    {
        var existingProduct = _products.FirstOrDefault(p => p.Id == product.Id);
        if (existingProduct != null)
        {
            _products.Remove(existingProduct);
            _products.Add(product);
        }
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Product product, CancellationToken cancellationToken = default)
    {
        var existingProduct = _products.FirstOrDefault(p => p.Id == product.Id);
        if (existingProduct != null)
        {
            _products.Remove(existingProduct);
        }
        return Task.CompletedTask;
    }

    public Task<IEnumerable<Product>> GetAllAsync(GetProductsQuery query, CancellationToken cancellationToken = default)
    {
        var products = _products.AsEnumerable();

        // Apply filtering based on the query parameters
        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            products = products.Where(p => p.Name.Contains(query.Search, StringComparison.OrdinalIgnoreCase) ||
                                           (p.Description != null && p.Description.Contains(query.Search, StringComparison.OrdinalIgnoreCase)));
        }

        if (query.Type != null)
        {
            products = products.Where(p => p.Type == query.Type);
        }

        if (query.Status != null)
        {
            products = products.Where(p => p.Status == query.Status);
        }

        if (query.MinPrice.HasValue)
        {
            products = products.Where(p => p.Price >= query.MinPrice.Value);
        }

        if (query.MaxPrice.HasValue)
        {
            products = products.Where(p => p.Price <= query.MaxPrice.Value);
        }

        if (query.CategoryId.HasValue)
        {
            products = products.Where(p => p.CategoryId == query.CategoryId.Value);
        }

        if (!string.IsNullOrWhiteSpace(query.Sort))
        {
            products = query.Sort.ToLower() switch
            {
                "name" => products.OrderBy(p => p.Name),
                "-name" => products.OrderByDescending(p => p.Name),
                "price" => products.OrderBy(p => p.Price),
                "-price" => products.OrderByDescending(p => p.Price),
                _ => products.OrderBy(p => p.Id)
            };
        }
        else
        {
            products = products.OrderBy(p => p.Id);
        }
        return Task.FromResult(products);
    }

    public Task<int> CountAsync(CancellationToken cancellationToken = default)
    {
        return Task.FromResult(_products.Count);
    }
}
