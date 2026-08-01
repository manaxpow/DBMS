public record UpdateCustomerRequest(
    string Name,
    string Email,
    string? Phone,
    string? Category,
    CustomerMemberType MemberType,
    CustomerStatus Status
);
