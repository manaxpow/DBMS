using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineStores.Services;

[ApiController]
[Route("api/v1/customers")]
[Authorize]
public class CustomersController(ICustomerService customerService) : ControllerBase
{
    private readonly ICustomerService _customerService = customerService;

    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<ActionResult<PagedResponse<CustomerResponse>>> GetCustomers(
        [FromQuery] GetCustomersQuery query, 
        CancellationToken cancellationToken)
    {
        var customers = await _customerService.GetCustomersAsync(query, cancellationToken);
        return Ok(customers);
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("{customerId}")]
    public async Task<ActionResult<CustomerResponse>> GetCustomerById(
        Guid customerId,
        [FromQuery] GetCustomerByIdQuery query,
        CancellationToken cancellationToken = default)
    {
        var customer = await _customerService.GetCustomerByIdAsync(customerId, query.IncludeUsers ?? false, query.IncludeStatistics ?? false, query.IncludeMetadata ?? false, cancellationToken);
        if (customer is null)
        {
            return NotFound();
        }

        return Ok(customer);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<ActionResult<CustomerResponse>> CreateCustomer(
        [FromBody] CreateCustomerRequest request,
        [FromQuery] CreateCustomerQuery query,
        CancellationToken cancellationToken = default)
    {
        var customer = await _customerService.CreateCustomerAsync(request, query.SendInvitation ?? false, cancellationToken);
        return CreatedAtAction(nameof(GetCustomerById), new { customerId = customer.Id }, customer);
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{customerId}")]
    public async Task<ActionResult<CustomerResponse>> UpdateCustomer(
        Guid customerId,
        [FromBody] UpdateCustomerRequest request,
        CancellationToken cancellationToken = default)
    {
        var customer = await _customerService.UpdateCustomerAsync(customerId, request, cancellationToken);
        return Ok(customer);
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{customerId}")]
    public async Task<ActionResult> DeleteCustomer(
        Guid customerId,
        [FromQuery] DeleteCustomerQuery query,
        CancellationToken cancellationToken = default)
    {
        await _customerService.DeleteCustomerAsync(customerId, query.Force ?? false, query.AnonymizeData ?? false, cancellationToken);
        return NoContent();
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("{customerId}/orders")]
    public async Task<IActionResult> GetCustomerOrders(
        Guid customerId,
        [FromQuery] GetCustomerOrdersQuery query,
        CancellationToken cancellationToken = default)
    {
        // Placeholder for when Orders module is implemented
        return Ok(new PagedResponse<object>(Array.Empty<object>(), 0, query.Page, query.PageSize));
    }
}
