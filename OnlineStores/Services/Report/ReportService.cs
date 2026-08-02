public class ReportService(IOrderRepository orderRepository) : IReportService
{
    private readonly IOrderRepository _orderRepository = orderRepository;

    public async Task<RevenueReportResponse> GetRevenueReportAsync(GetRevenueReportQuery query, CancellationToken cancellationToken = default)
    {
        var ordersQuery = new GetOrdersQuery(
            Search: null,
            CustomerId: null,
            Status: null,
            PaymentStatus: null,
            FulfillmentStatus: null,
            CreatedFrom: query.From,
            CreatedTo: query.To,
            MinTotal: null,
            MaxTotal: null,
            Sort: null,
            Page: 1,
            PageSize: 10000
        );

        var orders = await _orderRepository.GetOrdersAsync(ordersQuery, cancellationToken);

        var dataPoints = new List<RevenueDataPoint>();

        var interval = query.Interval?.ToLower() ?? "daily";

        var groupedOrders = orders.GroupBy(o =>
        {
            if (interval == "monthly")
            {
                return o.CreatedAt.ToString("yyyy-MM");
            }
            else if (interval == "yearly")
            {
                return o.CreatedAt.ToString("yyyy");
            }
            // default to daily
            return o.CreatedAt.ToString("yyyy-MM-dd");
        }).OrderBy(g => g.Key);

        foreach (var group in groupedOrders)
        {
            dataPoints.Add(new RevenueDataPoint(
                Label: group.Key,
                Revenue: group.Sum(o => o.TotalAmount),
                OrdersCount: group.Count()
            ));
        }

        var totalRevenue = dataPoints.Sum(d => d.Revenue);
        var totalOrders = dataPoints.Sum(d => d.OrdersCount);

        return new RevenueReportResponse(
            TotalRevenue: totalRevenue,
            TotalOrders: totalOrders,
            Currency: query.Currency ?? "USD",
            Data: dataPoints
        );
    }
}
