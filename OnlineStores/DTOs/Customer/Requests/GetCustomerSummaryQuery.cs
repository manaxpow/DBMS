public record GetCustomerSummaryQuery(
    DateTime? From,
    DateTime? To,
    string? Timezone
);
