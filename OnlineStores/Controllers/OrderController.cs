using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/v1/orders")]
public class OrderController(IOrderService orderService) : ControllerBase
{
    private readonly IOrderService _orderService = orderService;

    [Authorize(Roles = "Admin")]
    [HttpGet]
    [ProducesResponseType(typeof(PagedResponse<OrderResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<PagedResponse<OrderResponse>>> GetOrders(
        [FromQuery] GetOrdersQuery query,
        CancellationToken cancellationToken)
    {
        var orders = await _orderService.GetOrdersAsync(query, cancellationToken);
        return Ok(orders);
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("{orderId}")]
    public async Task<ActionResult<OrderResponse>> GetOrderById(
        Guid orderId,
        [FromQuery] bool includeItems = false,
        [FromQuery] bool includeCustomer = false,
        [FromQuery] bool includePayments = false,
        [FromQuery] bool includeHistory = false,
        CancellationToken cancellationToken = default)
    {
        var order = await _orderService.GetOrderByIdAsync(orderId, includeItems, includeCustomer, includePayments, includeHistory, cancellationToken);
        if (order is null)
        {
            return NotFound();
        }

        return Ok(order);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<ActionResult<OrderResponse>> CreateOrder(
        [FromBody] CreateOrderRequest request,
        CancellationToken cancellationToken = default)
    {
        var order = await _orderService.CreateOrderAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetOrderById), new { orderId = order.Id }, order);
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{orderId}")]
    public async Task<ActionResult<OrderResponse>> UpdateOrder(
        Guid orderId,
        [FromBody] UpdateOrderRequest request,
        CancellationToken cancellationToken = default)
    {
        var order = await _orderService.UpdateOrderAsync(orderId, request, cancellationToken);
        return Ok(order);
    }

    [Authorize(Roles = "Admin")]
    [HttpPatch("{orderId}/status")]
    public async Task<ActionResult<OrderResponse>> UpdateOrderStatus(
        Guid orderId,
        [FromBody] UpdateOrderStatusRequest request,
        CancellationToken cancellationToken = default)
    {
        var order = await _orderService.UpdateOrderStatusAsync(orderId, request, cancellationToken);
        return Ok(order);
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{orderId}")]
    public async Task<ActionResult> DeleteOrder(
        Guid orderId,
        CancellationToken cancellationToken = default)
    {
        await _orderService.DeleteOrderAsync(orderId, cancellationToken);
        return NoContent();
    }
}
