public record CustomerSummaryResponse(
    int TotalCustomers,
    decimal TotalCustomersChangePercentage,
    int TotalMembers,
    decimal TotalMembersChangePercentage,
    int ActiveNow
);
