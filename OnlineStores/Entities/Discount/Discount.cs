public class Discount
{
    public Guid Id { get; private set; }
    public string Code { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public DiscountType Type { get; private set; }
    public decimal Value { get; private set; }
    public DiscountStatus Status { get; private set; }
    public DateTime StartsAt { get; private set; }
    public DateTime? ExpiresAt { get; private set; }
    public int? UsageLimit { get; private set; }
    public int UsageCount { get; private set; }
    public decimal? MinimumRequirementValue { get; private set; }

    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }


    public static Discount Create(
        string code,
        string description,
        DiscountType type,
        decimal value,
        DateTime startsAt,
        DateTime? expiresAt,
        int? usageLimit,
        decimal? minimumRequirementValue)
    {
        return new Discount
        {
            Id = Guid.NewGuid(),
            Code = code.ToUpperInvariant(),
            Description = description,
            Type = type,
            Value = value,
            Status = startsAt <= DateTime.UtcNow ? DiscountStatus.Active : DiscountStatus.Scheduled,
            StartsAt = startsAt,
            ExpiresAt = expiresAt,
            UsageLimit = usageLimit,
            UsageCount = 0,
            MinimumRequirementValue = minimumRequirementValue,
            CreatedAt = DateTime.UtcNow
        };
    }

    public void Update(
        string code,
        string description,
        DiscountType type,
        decimal value,
        DateTime startsAt,
        DateTime? expiresAt,
        int? usageLimit,
        decimal? minimumRequirementValue)
    {
        Code = code.ToUpperInvariant();
        Description = description;
        Type = type;
        Value = value;
        StartsAt = startsAt;
        ExpiresAt = expiresAt;
        UsageLimit = usageLimit;
        MinimumRequirementValue = minimumRequirementValue;
        UpdatedAt = DateTime.UtcNow;
    }

    public void SetStatus(DiscountStatus status)
    {
        Status = status;
        UpdatedAt = DateTime.UtcNow;
    }
}
