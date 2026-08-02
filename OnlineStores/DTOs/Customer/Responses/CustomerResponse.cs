public record CustomerResponse(
    Guid Id,
    string CompanyName,
    string? LogoUrl,
    string? Domain,
    CustomerStatus Status,
    string? Category,
    string? Description,
    int UserCount,
    IReadOnlyList<CustomerUserSummaryResponse> Users,
    DateTime CreatedAt,
    DateTime? LastActiveAt
);
