public record UpdateCustomerRequest(
    string CompanyName,
    string? Domain,
    CustomerStatus Status,
    string? Category,
    string? Description
);
