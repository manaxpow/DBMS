public interface IDiscountService
{
    Task<PagedResponse<DiscountResponse>> GetDiscountsAsync(GetDiscountsQuery query, CancellationToken cancellationToken = default);
    Task<DiscountResponse?> GetDiscountByIdAsync(Guid discountId, bool includeUsageStatistics = false, bool includeProducts = false, CancellationToken cancellationToken = default);
    Task<DiscountResponse> CreateDiscountAsync(CreateDiscountRequest request, bool activateImmediately = false, CancellationToken cancellationToken = default);
    Task<DiscountResponse> UpdateDiscountAsync(Guid discountId, UpdateDiscountRequest request, CancellationToken cancellationToken = default);
    Task DeleteDiscountAsync(Guid discountId, bool force = false, CancellationToken cancellationToken = default);
}
