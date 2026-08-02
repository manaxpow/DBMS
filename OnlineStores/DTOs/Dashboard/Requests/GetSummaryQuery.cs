public record GetSummaryQuery(
    DateTime? From,
    DateTime? To,
    string? Timezone,
    string? Currency
);
