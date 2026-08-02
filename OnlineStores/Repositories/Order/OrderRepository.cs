public class OrderRepository : IOrderRepository
{
    private List<Order> _orders = new List<Order>();

    public Task AddAsync(Order order, CancellationToken cancellationToken = default)
    {
        _orders.Add(order);
        return Task.CompletedTask;
    }

    public Task<int> CountAsync(GetOrdersQuery query, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(_orders.Count);
    }

    public Task DeleteAsync(Order order, CancellationToken cancellationToken = default)
    {
        _orders.Remove(order);
        return Task.CompletedTask;
    }

    public Task<Order?> GetByIdAsync(Guid id, bool includeItems = false, bool includeCustomer = false, bool includePayments = false, bool includeHistory = false, CancellationToken cancellationToken = default)
    {
        if (includeItems || includeCustomer || includePayments || includeHistory)
        {
            // Projection
        }
        var order = _orders.FirstOrDefault(o => o.Id == id);
        return Task.FromResult(order);
    }

    public Task<IEnumerable<Order>> GetOrdersAsync(GetOrdersQuery query, CancellationToken cancellationToken = default)
    {
        IEnumerable<Order> orders = _orders;

        if (!string.IsNullOrEmpty(query.Search))
        {
            orders = orders.Where(o => o.Id.ToString().Contains(query.Search, StringComparison.OrdinalIgnoreCase));
        }

        if (query.CustomerId.HasValue)
        {
            orders = orders.Where(o => o.CustomerId == query.CustomerId.Value);
        }

        if (query.Status.HasValue)
        {
            orders = orders.Where(o => o.Status == query.Status.Value);
        }

        if (query.MinTotal.HasValue)
        {
            orders = orders.Where(o => o.TotalAmount >= query.MinTotal.Value);
        }

        if (query.MaxTotal.HasValue)
        {
            orders = orders.Where(o => o.TotalAmount <= query.MaxTotal.Value);
        }

        return Task.FromResult(orders);
    }

    public Task<OrdersSummary> GetSummaryDataAsync(DateTime? from, DateTime? to, string? timezone, string? currency, CancellationToken cancellationToken = default)
    {
        var filteredOrders = _orders.AsEnumerable();

        if (from.HasValue)
        {
            filteredOrders = filteredOrders.Where(o => o.CreatedAt >= from.Value);
        }

        if (to.HasValue)
        {
            filteredOrders = filteredOrders.Where(o => o.CreatedAt <= to.Value);
        }

        var totalOrders = filteredOrders.Count();
        var totalRevenue = filteredOrders.Sum(o => o.TotalAmount);

        return Task.FromResult(new OrdersSummary
        (
            TotalOrders: totalOrders,
            TotalRevenue: totalRevenue
        )
        );
    }

    public Task UpdateAsync(Order order, CancellationToken cancellationToken = default)
    {
        var existingOrder = _orders.FirstOrDefault(o => o.Id == order.Id);
        if (existingOrder != null)
        {
            _orders.Remove(existingOrder);
            _orders.Add(order);
        }
        return Task.CompletedTask;
    }

    public Task UpdateStatusAsync(Guid id, OrderStatus status, CancellationToken cancellationToken = default)
    {
        var order = _orders.FirstOrDefault(o => o.Id == id);
        if (order != null)
        {
            order.UpdateStatus(status);
        }
        return Task.CompletedTask;
    }
}
