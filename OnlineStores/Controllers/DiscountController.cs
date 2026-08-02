using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/v1/discounts")]
[Authorize]
public class DiscountController : ControllerBase
{
    private readonly IDiscountService _discountService;

    public DiscountController(IDiscountService discountService)
    {
        _discountService = discountService;
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(PagedResponse<DiscountResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<PagedResponse<DiscountResponse>>> GetDiscounts(
        [FromQuery] GetDiscountsQuery query,
        CancellationToken cancellationToken)
    {
        var discounts = await _discountService.GetDiscountsAsync(query, cancellationToken);
        return Ok(discounts);
    }

    [HttpGet("{discountId}")]
    [Authorize(Roles = "Admin, User")]
    [ProducesResponseType(typeof(DiscountResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<DiscountResponse>> GetDiscountById(
        Guid discountId,
        [FromQuery] bool includeUsageStatistics = false,
        [FromQuery] bool includeProducts = false,
        CancellationToken cancellationToken = default)
    {
        var discount = await _discountService.GetDiscountByIdAsync(discountId, includeUsageStatistics, includeProducts, cancellationToken);
        if (discount is null)
        {
            return NotFound();
        }
        return Ok(discount);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(DiscountResponse), StatusCodes.Status201Created)]
    public async Task<ActionResult<DiscountResponse>> CreateDiscount(
        [FromQuery] bool activateImmediately,
        [FromBody] CreateDiscountRequest request,
        CancellationToken cancellationToken = default)
    {
        var discount = await _discountService.CreateDiscountAsync(request, activateImmediately, cancellationToken);
        return CreatedAtAction(nameof(GetDiscountById), new { discountId = discount.Id }, discount);
    }

    [HttpPut("{discountId}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(DiscountResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<DiscountResponse>> UpdateDiscount(
        Guid discountId,
        [FromBody] UpdateDiscountRequest request,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var discount = await _discountService.UpdateDiscountAsync(discountId, request, cancellationToken);
            return Ok(discount);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpDelete("{discountId}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> DeleteDiscount(
        Guid discountId,
        [FromQuery] bool force = false,
        CancellationToken cancellationToken = default)
    {
        try
        {
            await _discountService.DeleteDiscountAsync(discountId, force, cancellationToken);
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
