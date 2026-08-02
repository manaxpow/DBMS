public interface IDiscountRepository
{
    Task<Discount?> GetByIdAsync(Guid discountId, bool includeUsageStatistics = false, bool includeProducts = false, CancellationToken cancellationToken = default);
    Task<(IEnumerable<Discount> Items, int TotalCount)> GetPagedAsync(GetDiscountsQuery query, CancellationToken cancellationToken = default);
    Task<Discount?> GetByCodeAsync(string code, CancellationToken cancellationToken = default);
    Task AddAsync(Discount discount, CancellationToken cancellationToken = default);
    Task UpdateAsync(Discount discount, CancellationToken cancellationToken = default);
    Task DeleteAsync(Discount discount, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(string code, CancellationToken cancellationToken = default);
}
