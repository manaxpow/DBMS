using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/v1/orders")]
public class OrderController(IOrderService orderService) : ControllerBase
{
    private readonly IOrderService _orderService = orderService;

    [Authorize(Roles = "Admin, User")]
    [HttpGet]
    [ProducesResponseType(typeof(PagedResponse<OrderResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<PagedResponse<OrderResponse>>> GetOrders(
        [FromQuery] GetOrdersQuery query,
        CancellationToken cancellationToken)
    {
        var userRole = User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.Role)?.Value;
        var userIdString = User.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;

        if (userRole != "Admin")
        {
            if (!Guid.TryParse(userIdString, out var userId))
            {
                return Unauthorized();
            }
            query = query with { CustomerId = userId };
        }

        var orders = await _orderService.GetOrdersAsync(query, cancellationToken);
        return Ok(orders);
    }

    [Authorize(Roles = "Admin, User")]
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

        var userRole = User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.Role)?.Value;
        var userIdString = User.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
        
        if (userRole != "Admin")
        {
            if (!Guid.TryParse(userIdString, out var userId) || order.CustomerId != userId)
            {
                return Forbid();
            }
        }

        return Ok(order);
    }

    [Authorize(Roles = "Admin, User")]
    [HttpPost]
    public async Task<ActionResult<OrderResponse>> CreateOrder(
        [FromBody] CreateOrderRequest request,
        CancellationToken cancellationToken = default)
    {
        var userRole = User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.Role)?.Value;
        if (userRole != "Admin")
        {
            var userIdString = User.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
            if (!Guid.TryParse(userIdString, out var userId))
            {
                return Unauthorized();
            }
            // For regular users, ensure they are creating order for themselves
            request = request with { CustomerId = userId };
        }

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
