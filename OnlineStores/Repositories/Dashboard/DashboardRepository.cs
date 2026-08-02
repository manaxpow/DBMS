public class DashboardRepository(IOrderRepository orderRepository, IProductRepository productRepository) : IDashboardRepository
{
    private readonly IOrderRepository _orderRepository = orderRepository;
    private readonly IProductRepository _productRepository = productRepository;

    public async Task<SummaryResponse> GetSummaryDataAsync(DateTime? from, DateTime? to, string? timezone, string? currency, CancellationToken cancellationToken = default)
    {
        var totalSalesTask = await _orderRepository.GetSummaryDataAsync(from, to, timezone, currency, cancellationToken);
        var totalCustomersTask = await _productRepository.CountAsync(cancellationToken);
        var totalProductsTask = await _productRepository.CountAsync(cancellationToken);

        var averageOrderValue = totalSalesTask.TotalOrders > 0 ? totalSalesTask.TotalRevenue / totalSalesTask.TotalOrders : 0;

        return new SummaryResponse
        (
            TotalSales: totalSalesTask.TotalRevenue,
            OrdersSummary: totalSalesTask,
            TotalCustomers: totalCustomersTask,
            TotalProducts: totalProductsTask,
            AverageOrderValue: averageOrderValue,
            SalesTimeline: new List<DailySales>()
        );
    }
}
