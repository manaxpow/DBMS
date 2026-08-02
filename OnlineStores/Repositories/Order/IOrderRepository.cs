public interface IOrderRepository
{
    Task<IEnumerable<Order>> GetOrdersAsync(GetOrdersQuery query, CancellationToken cancellationToken = default);
    Task<int> CountAsync(GetOrdersQuery query, CancellationToken cancellationToken = default);
    Task AddAsync(Order order, CancellationToken cancellationToken = default);
    Task<Order?> GetByIdAsync(Guid id, bool includeItems = false, bool includeCustomer = false, bool includePayments = false, bool includeHistory = false, CancellationToken cancellationToken = default);
    Task UpdateAsync(Order order, CancellationToken cancellationToken = default);
    Task UpdateStatusAsync(Guid id, OrderStatus status, CancellationToken cancellationToken = default);
    Task DeleteAsync(Order order, CancellationToken cancellationToken = default);
}
