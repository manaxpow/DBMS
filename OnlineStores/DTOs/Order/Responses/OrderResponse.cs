public record OrderResponse(
    Guid Id,
    Guid CustomerId,
    decimal TotalAmount,
    OrderStatus Status,
    DateTime CreatedAt,
    List<OrderItemResponse> Items
);
