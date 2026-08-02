public class DiscountRepository : IDiscountRepository
{
    private readonly List<Discount> _discounts = new();

    public Task AddAsync(Discount discount, CancellationToken cancellationToken = default)
    {
        var existingDiscount = _discounts.FirstOrDefault(d => d.Code == discount.Code);
        if (existingDiscount != null)
        {
            throw new InvalidOperationException($"A discount with the code '{discount.Code}' already exists.");
        }
        _discounts.Add(discount);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Discount discount, CancellationToken cancellationToken = default)
    {
        _discounts.Remove(discount);
        return Task.CompletedTask;
    }

    public Task<bool> ExistsAsync(string code, CancellationToken cancellationToken = default)
    {
        var exists = _discounts.Any(d => d.Code == code);
        return Task.FromResult(exists);
    }

    public Task<Discount?> GetByCodeAsync(string code, CancellationToken cancellationToken = default)
    {
        var discount = _discounts.FirstOrDefault(d => d.Code == code);
        return Task.FromResult(discount);
    }

    public Task<Discount?> GetByIdAsync(Guid discountId, bool includeUsageStatistics = false, bool includeProducts = false, CancellationToken cancellationToken = default)
    {
        var discount = _discounts.FirstOrDefault(d => d.Id == discountId);
        return Task.FromResult(discount);
    }

    public Task<(IEnumerable<Discount> Items, int TotalCount)> GetPagedAsync(GetDiscountsQuery query, CancellationToken cancellationToken = default)
    {
        var filteredDiscounts = _discounts.AsQueryable();

        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            filteredDiscounts = filteredDiscounts.Where(d => d.Code.Contains(query.Search, StringComparison.OrdinalIgnoreCase));
        }

        if (query.Status.HasValue)
        {
            filteredDiscounts = filteredDiscounts.Where(d => d.Status == query.Status.Value);
        }

        var totalCount = filteredDiscounts.Count();

        var pagedDiscounts = filteredDiscounts
            .Skip((query.Page - 1) * query.PageSize)
            .Take(query.PageSize)
            .ToList();

        return Task.FromResult((pagedDiscounts.AsEnumerable(), totalCount));
    }
    public Task UpdateAsync(Discount discount, CancellationToken cancellationToken = default)
    {
        var existingDiscount = _discounts.FirstOrDefault(d => d.Id == discount.Id);
        if (existingDiscount == null)
        {
            throw new InvalidOperationException($"Discount with ID '{discount.Id}' does not exist.");
        }

        existingDiscount.Update(
            discount.Code,
            discount.Description,
            discount.Type,
            discount.Value,
            discount.StartsAt,
            discount.ExpiresAt,
            discount.UsageLimit,
            discount.MinimumRequirementValue);

        return Task.CompletedTask;
    }
}
