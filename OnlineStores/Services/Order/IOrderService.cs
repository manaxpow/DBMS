

public interface IOrderService
{
    Task<PagedResponse<OrderResponse>> GetOrdersAsync(GetOrdersQuery query, CancellationToken cancellationToken = default);
    Task<OrderResponse?> GetOrderByIdAsync(Guid orderId, bool includeItems = false, bool includeCustomer = false, bool includePayments = false, bool includeHistory = false, CancellationToken cancellationToken = default);
    Task<OrderResponse> CreateOrderAsync(CreateOrderRequest request, CancellationToken cancellationToken = default);
    Task<OrderResponse> UpdateOrderAsync(Guid orderId, UpdateOrderRequest request, CancellationToken cancellationToken = default);
    Task<OrderResponse> UpdateOrderStatusAsync(Guid orderId, UpdateOrderStatusRequest request, CancellationToken cancellationToken = default);
    Task DeleteOrderAsync(Guid orderId, CancellationToken cancellationToken = default);
}
