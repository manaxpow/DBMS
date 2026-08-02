public record CreateDiscountRequest(
    string Code,
    string Description,
    DiscountType Type,
    decimal Value,
    DateTime StartsAt,
    DateTime? ExpiresAt,
    int? UsageLimit,
    decimal? MinimumRequirementValue
);
