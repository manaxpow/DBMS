using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/v1/stores")]
public class StoreController(IStoreService storeService) : ControllerBase
{
    private readonly IStoreService _storeService = storeService;

    [Authorize(Roles = "Owner")]
    [HttpGet]
    public async Task<IActionResult> GetMyStore(
        [FromQuery] bool includeOwner = false,
        [FromQuery] bool includeSettings = false,
        CancellationToken cancellationToken = default)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId is null)
        {
            return Unauthorized();
        }

        var store = await _storeService.GetStoreAsync(Guid.Parse(userId), includeOwner, includeSettings, cancellationToken);
        if (store is null)
        {
            return NotFound();
        }

        return Ok(store);
    }

    [Authorize(Roles = "Owner")]
    [HttpPut]
    public async Task<IActionResult> UpdateMyStore([FromBody] UpdateStoreRequest request, CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId is null)
        {
            return Unauthorized();
        }

        var updatedStore = await _storeService.UpdateStoreAsync(Guid.Parse(userId), request, cancellationToken);
        return Ok(updatedStore);
    }

    [Authorize(Roles = "Owner")]
    [HttpPost("logo")]
    public async Task<IActionResult> UpdateMyStoreLogo(
        [FromForm] UpdateStoreLogoRequest request,
        [FromQuery] bool replaceExisting = false,
        CancellationToken cancellationToken = default)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId is null)
        {
            return Unauthorized();
        }

        var updatedStore = await _storeService.UpdateStoreLogoAsync(Guid.Parse(userId), request.LogoFile, replaceExisting, cancellationToken);
        return Ok(updatedStore);
    }
}
