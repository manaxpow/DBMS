public record DiscountResponse(
    Guid Id,
    string Code,
    string Description,
    DiscountType Type,
    decimal Value,
    DiscountStatus Status,
    DateTime StartsAt,
    DateTime? ExpiresAt,
    int? UsageLimit,
    int UsageCount,
    decimal? MinimumRequirementValue,
    DateTime CreatedAt,
    DateTime? UpdatedAt
);
