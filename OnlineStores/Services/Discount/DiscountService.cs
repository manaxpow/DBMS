public class DiscountService(IDiscountRepository discountRepository) : IDiscountService
{
    private readonly IDiscountRepository _discountRepository = discountRepository;

    public async Task<PagedResponse<DiscountResponse>> GetDiscountsAsync(GetDiscountsQuery query, CancellationToken cancellationToken = default)
    {
        var (items, totalCount) = await _discountRepository.GetPagedAsync(query, cancellationToken);
        var discountResponses = items.Select(discount => MapToResponse(discount)).ToList();
        return new PagedResponse<DiscountResponse>(discountResponses, totalCount, query.Page, query.PageSize);
    }

    public async Task<DiscountResponse?> GetDiscountByIdAsync(Guid discountId, bool includeUsageStatistics = false, bool includeProducts = false, CancellationToken cancellationToken = default)
    {
        var discount = await _discountRepository.GetByIdAsync(discountId, includeUsageStatistics, includeProducts, cancellationToken);
        return discount is not null ? MapToResponse(discount) : null;
    }

    public async Task<DiscountResponse> CreateDiscountAsync(CreateDiscountRequest request, bool activateImmediately = false, CancellationToken cancellationToken = default)
    {
        var discount = Discount.Create(
            code: request.Code,
            description: request.Description,
            type: request.Type,
            value: request.Value,
            startsAt: activateImmediately ? DateTime.UtcNow : request.StartsAt,
            expiresAt: request.ExpiresAt,
            usageLimit: request.UsageLimit,
            minimumRequirementValue: request.MinimumRequirementValue
        );

        await _discountRepository.AddAsync(discount, cancellationToken);
        return MapToResponse(discount);
    }

    public async Task<DiscountResponse> UpdateDiscountAsync(Guid discountId, UpdateDiscountRequest request, CancellationToken cancellationToken = default)
    {
        var existingDiscount = await _discountRepository.GetByIdAsync(discountId, false, false, cancellationToken);
        if (existingDiscount is null)
        {
            throw new KeyNotFoundException($"Discount with ID {discountId} not found.");
        }

        existingDiscount.Update(
            code: request.Code,
            description: request.Description,
            type: request.Type,
            value: request.Value,
            startsAt: request.StartsAt,
            expiresAt: request.ExpiresAt,
            usageLimit: request.UsageLimit,
            minimumRequirementValue: request.MinimumRequirementValue
        );

        await _discountRepository.UpdateAsync(existingDiscount, cancellationToken);
        return MapToResponse(existingDiscount);
    }

    public async Task DeleteDiscountAsync(Guid discountId, bool force = false, CancellationToken cancellationToken = default)
    {
        var existingDiscount = await _discountRepository.GetByIdAsync(discountId, false, false, cancellationToken);
        if (existingDiscount is null
)
        {
            throw new KeyNotFoundException($"Discount with ID {discountId} not found.");
        }
        if (!force && existingDiscount.Status == DiscountStatus.Active)
        {
            throw new InvalidOperationException("Cannot delete an active discount. Use force delete to remove it.");
        }
        await _discountRepository.DeleteAsync(existingDiscount, cancellationToken);
    }

    private DiscountResponse MapToResponse(Discount discount)
    {
        return new DiscountResponse
        (
            Id: discount.Id,
            Code: discount.Code,
            Description: discount.Description,
            Type: discount.Type,
            Value: discount.Value,
            Status: discount.Status,
            StartsAt: discount.StartsAt,
            ExpiresAt: discount.ExpiresAt,
            CreatedAt: discount.CreatedAt,
            UpdatedAt: discount.UpdatedAt,
            UsageLimit: discount.UsageLimit,
            UsageCount: discount.UsageCount,
            MinimumRequirementValue: discount.MinimumRequirementValue
        );
    }
}
