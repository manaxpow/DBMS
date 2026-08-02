public record CreateCustomerUserRequest(
    string FullName,
    string Email,
    string? AvatarUrl,
    CustomerMemberRole Role
);
