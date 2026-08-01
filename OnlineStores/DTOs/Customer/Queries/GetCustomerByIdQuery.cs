public record GetCustomerByIdQuery(
    bool? IncludeUsers = false,
    bool? IncludeStatistics = false,
    bool? IncludeMetadata = false
);
