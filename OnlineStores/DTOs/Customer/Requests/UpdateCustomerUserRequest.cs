public record UpdateCustomerUserRequest(
    string FullName,
    string? AvatarUrl,
    CustomerMemberRole Role,
    CustomerMemberStatus Status
);
