public record GetCustomersQuery(
    string? Search,
    CustomerStatus? Status,
    string? Category,
    CustomerMemberType? MemberType,
    DateTime? CreatedFrom,
    DateTime? CreatedTo,
    DateTime? LastActiveFrom,
    DateTime? LastActiveTo,
    string? Sort,
    int Page = 1,
    int PageSize = 20
);
