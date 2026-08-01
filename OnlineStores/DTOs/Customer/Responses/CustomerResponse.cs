public record CustomerResponse(
    Guid Id,
    string Name,
    string Email,
    string? Phone,
    CustomerStatus Status,
    CustomerMemberType MemberType,
    string? Category,
    DateTime CreatedAt,
    DateTime? UpdatedAt,
    DateTime? LastActiveAt
);
