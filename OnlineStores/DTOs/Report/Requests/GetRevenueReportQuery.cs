public record GetRevenueReportQuery(
    DateTime? From,
    DateTime? To,
    string? Interval = "daily",
    string? Currency = "USD",
    string? Timezone = "UTC",
    string? GroupBy = null
);
