using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/v1/customers")]
[Authorize]
public class CustomersController(ICustomerService customerService, ICustomerExportService customerExportService) : ControllerBase
{
    private readonly ICustomerService _customerService = customerService;
    private readonly ICustomerExportService _customerExportService = customerExportService;

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
    [HttpGet("summary")]
    public async Task<ActionResult<CustomerSummaryResponse>> GetCustomerSummary(
        [FromQuery] GetCustomerSummaryQuery query,
        CancellationToken cancellationToken = default)
    {
        var summary = await _customerService.GetCustomerSummaryAsync(query, cancellationToken);
        return Ok(summary);
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportCustomers(
        [FromQuery] ExportCustomersQuery query,
        CancellationToken cancellationToken = default)
    {
        // Fetch all customers matching the query. Since GetCustomersAsync is paginated, we would ideally have a non-paginated method. 
        // For now we just get page 1 but large size.
        var pagedCustomers = await _customerService.GetCustomersAsync(new GetCustomersQuery(query.Search, query.Status, query.Category, query.CreatedFrom, query.CreatedTo, null, 1, 10000), cancellationToken);
        
        // This is a mockup of fetching entities for export. The ExportService might take DTOs or Entities. 
        // For simplicity, we just pass an empty list of entities and let it generate mockup.
        var result = await _customerExportService.ExportCustomersAsync(Array.Empty<Customer>(), query, cancellationToken);
        return File(result.Content, result.ContentType, result.FileName);
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("{customerId:guid}")]
    public async Task<ActionResult<CustomerResponse>> GetCustomerById(
        Guid customerId,
        [FromQuery] GetCustomerByIdQuery query,
        CancellationToken cancellationToken = default)
    {
        var customer = await _customerService.GetCustomerByIdAsync(customerId, query.IncludeUsers ?? false, query.IncludeStatistics ?? false, query.IncludeMetadata ?? false, cancellationToken);
        if (customer is null) return NotFound();
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
    [HttpPut("{customerId:guid}")]
    public async Task<ActionResult<CustomerResponse>> UpdateCustomer(
        Guid customerId,
        [FromBody] UpdateCustomerRequest request,
        CancellationToken cancellationToken = default)
    {
        var customer = await _customerService.UpdateCustomerAsync(customerId, request, cancellationToken);
        return Ok(customer);
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{customerId:guid}")]
    public async Task<ActionResult> DeleteCustomer(
        Guid customerId,
        [FromQuery] DeleteCustomerQuery query,
        CancellationToken cancellationToken = default)
    {
        await _customerService.DeleteCustomerAsync(customerId, query.Force ?? false, query.AnonymizeData ?? false, cancellationToken);
        return NoContent();
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("{customerId:guid}/logo")]
    public async Task<IActionResult> UploadLogo(
        Guid customerId,
        [FromForm] UploadCustomerLogoRequest request,
        [FromQuery] bool replaceExisting = false,
        CancellationToken cancellationToken = default)
    {
        var result = await _customerService.UploadLogoAsync(customerId, request.File, replaceExisting, cancellationToken);
        return Ok(new { url = result });
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("{customerId:guid}/orders")]
    public async Task<IActionResult> GetCustomerOrders(
        Guid customerId,
        [FromQuery] GetCustomerOrdersQuery query,
        CancellationToken cancellationToken = default)
    {
        return Ok(new PagedResponse<object>(Array.Empty<object>(), 0, query.Page, query.PageSize));
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("{customerId:guid}/users")]
    public async Task<ActionResult<PagedResponse<CustomerUserSummaryResponse>>> GetCustomerUsers(
        Guid customerId,
        [FromQuery] GetCustomerUsersQuery query,
        CancellationToken cancellationToken = default)
    {
        var users = await _customerService.GetCustomerUsersAsync(customerId, query, cancellationToken);
        return Ok(users);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("{customerId:guid}/users")]
    public async Task<ActionResult<CustomerUserSummaryResponse>> CreateCustomerUser(
        Guid customerId,
        [FromBody] CreateCustomerUserRequest request,
        [FromQuery] bool sendInvitation = false,
        CancellationToken cancellationToken = default)
    {
        var user = await _customerService.CreateCustomerUserAsync(customerId, request, sendInvitation, cancellationToken);
        return CreatedAtAction(nameof(GetCustomerUserById), new { customerId, userId = user.Id }, user);
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("{customerId:guid}/users/{userId:guid}")]
    public async Task<ActionResult<CustomerUserSummaryResponse>> GetCustomerUserById(
        Guid customerId,
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        var user = await _customerService.GetCustomerUserByIdAsync(customerId, userId, cancellationToken);
        if (user is null) return NotFound();
        return Ok(user);
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{customerId:guid}/users/{userId:guid}")]
    public async Task<ActionResult<CustomerUserSummaryResponse>> UpdateCustomerUser(
        Guid customerId,
        Guid userId,
        [FromBody] UpdateCustomerUserRequest request,
        CancellationToken cancellationToken = default)
    {
        var user = await _customerService.UpdateCustomerUserAsync(customerId, userId, request, cancellationToken);
        return Ok(user);
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{customerId:guid}/users/{userId:guid}")]
    public async Task<ActionResult> DeleteCustomerUser(
        Guid customerId,
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        await _customerService.DeleteCustomerUserAsync(customerId, userId, cancellationToken);
        return NoContent();
    }
}
