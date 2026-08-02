

public record UpdateOrderStatusRequest(
    OrderStatus Status,
    bool NotifyCustomer = false
);
