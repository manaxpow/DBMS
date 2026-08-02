public record RevenueReportResponse(
    decimal TotalRevenue,
    int TotalOrders,
    string Currency,
    List<RevenueDataPoint> Data
);
