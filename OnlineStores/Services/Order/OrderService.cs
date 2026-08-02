public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;

    public OrderService(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<OrderResponse> CreateOrderAsync(CreateOrderRequest request, CancellationToken cancellationToken = default)
    {
        var order = Order.Create(request.CustomerId);
        await _orderRepository.AddAsync(order, cancellationToken);
        return MapToOrderResponse(order);
    }

    public async Task DeleteOrderAsync(Guid orderId, CancellationToken cancellationToken = default)
    {
        var order = await _orderRepository.GetByIdAsync(orderId, cancellationToken: cancellationToken);
        if (order == null)
        {
            throw new KeyNotFoundException($"Order with ID {orderId} not found.");
        }
        await _orderRepository.DeleteAsync(order, cancellationToken);
    }

    public async Task<OrderResponse?> GetOrderByIdAsync(Guid orderId, bool includeItems = false, bool includeCustomer = false, bool includePayments = false, bool includeHistory = false, CancellationToken cancellationToken = default)
    {
        var order = await _orderRepository.GetByIdAsync(orderId, includeItems, includeCustomer, includePayments, includeHistory, cancellationToken);
        if (order == null)
        {
            return null;
        }
        return MapToOrderResponse(order);
    }

    public async Task<PagedResponse<OrderResponse>> GetOrdersAsync(GetOrdersQuery query, CancellationToken cancellationToken = default)
    {
        var orders = await _orderRepository.GetOrdersAsync(query, cancellationToken);
        var totalCount = await _orderRepository.CountAsync(query, cancellationToken);

        var orderResponses = orders.Select(MapToOrderResponse).ToList();

        return new PagedResponse<OrderResponse>(orderResponses, totalCount, query.Page, query.PageSize);
    }

    public Task<OrderResponse> UpdateOrderAsync(Guid orderId, UpdateOrderRequest request, CancellationToken cancellationToken = default)
    {
        var order = _orderRepository.GetByIdAsync(orderId, cancellationToken: cancellationToken).Result;
        if (order == null)
        {
            throw new KeyNotFoundException($"Order with ID {orderId} not found.");
        }

        if (request.Items == null || !request.Items.Any())
        {
            throw new ArgumentException("Order must have at least one item.");
        }

        var updatedItems = request.Items.Select(i => OrderItem.Create(orderId, i.ProductId, i.Quantity, i.UnitPrice)).ToList();
        order.UpdateItems(updatedItems);
        _orderRepository.UpdateAsync(order, cancellationToken);
        return Task.FromResult(MapToOrderResponse(order));
    }

    public async Task<OrderResponse> UpdateOrderStatusAsync(Guid orderId, UpdateOrderStatusRequest request, CancellationToken cancellationToken = default)
    {
        var order = await _orderRepository.GetByIdAsync(orderId, cancellationToken: cancellationToken)  ;
        if (order == null)
        {
            throw new NotFoundException($"Order with ID {orderId} not found.");
        }

        order.UpdateStatus(request.Status);
        await _orderRepository.UpdateStatusAsync(orderId, request.Status, cancellationToken);
        return MapToOrderResponse(order);
    }

    private OrderResponse MapToOrderResponse(Order order)
    {
        return new OrderResponse
        (
            Id: order.Id,
            CustomerId: order.CustomerId,
            Status: order.Status,
            CreatedAt: order.CreatedAt,
            TotalAmount: order.TotalAmount,
            Items: order.Items.Select(i => new OrderItemResponse
            (
                Id: i.Id,
                ProductId: i.ProductId,
                Quantity: i.Quantity,
                UnitPrice: i.UnitPrice,
                TotalPrice: i.TotalPrice
            )).ToList()
        );
    }
}
