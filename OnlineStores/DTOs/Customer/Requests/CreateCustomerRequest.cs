public record CreateCustomerRequest(
    string Name,
    string Email,
    string? Phone,
    string? Category,
    CustomerMemberType MemberType = CustomerMemberType.Regular
);
