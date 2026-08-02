public record GetCustomerUsersQuery(
    string? Search,
    CustomerMemberStatus? Status,
    CustomerMemberRole? Role,
    string? Sort,
    int Page = 1,
    int PageSize = 20
);
