public record StoreResponse(
    Guid Id,
    Guid OwnerId,
    string Name,
    string? Description,
    string? LogoUrl,
    DateTime CreatedAt,
    DateTime? UpdatedAt
);
