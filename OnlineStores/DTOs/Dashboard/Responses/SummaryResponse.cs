public record SummaryResponse(
    decimal TotalSales,
    OrdersSummary OrdersSummary,
    int TotalCustomers,
    int TotalProducts,
    decimal AverageOrderValue,
    List<DailySales> SalesTimeline
);

public record OrdersSummary(
    int TotalOrders,
    decimal TotalRevenue
);
