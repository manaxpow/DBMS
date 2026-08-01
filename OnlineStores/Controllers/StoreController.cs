using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/v1/stores")]
public class StoreController(IStoreService storeService) : ControllerBase
{
    private readonly IStoreService _storeService = storeService;

    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<IActionResult> GetMyStore(
        [FromQuery] GetMyStoreQuery query,
        CancellationToken cancellationToken = default)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId is null)
        {
            return Unauthorized();
        }

        var store = await _storeService.GetStoreAsync(Guid.Parse(userId), query.IncludeOwner ?? false, query.IncludeSettings ?? false, cancellationToken);
        if (store is null)  
        {
            return NotFound();
        }

        return Ok(store);
    }

    [Authorize(Roles = "Admin")]
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

    [Authorize(Roles = "Admin")]
    [HttpPost("logo")]
    public async Task<IActionResult> UpdateMyStoreLogo(
        [FromForm] UpdateStoreLogoRequest request,
        [FromQuery] UpdateMyStoreLogoQuery query,
        CancellationToken cancellationToken = default)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId is null)
        {
            return Unauthorized();
        }

        var updatedStore = await _storeService.UpdateStoreLogoAsync(Guid.Parse(userId), request.LogoFile, query.ReplaceExisting ?? false, cancellationToken);
        return Ok(updatedStore);
    }
}
