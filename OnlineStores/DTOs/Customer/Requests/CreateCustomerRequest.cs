public record CreateCustomerRequest(
    string CompanyName,
    string? Domain,
    CustomerStatus Status,
    string? Category,
    string? Description,
    List<CreateCustomerUserRequest>? Users
);
