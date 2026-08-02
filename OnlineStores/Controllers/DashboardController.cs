using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/v1/overview")]
public class DashboardController(IDashboardService dashboardService) : ControllerBase
{
    private readonly IDashboardService _dashboardService = dashboardService;

    [Authorize(Roles = "Admin")]
    [HttpGet("summary")]
    [ProducesResponseType(typeof(SummaryResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<SummaryResponse>> GetSummary(
        [FromQuery] GetSummaryQuery query,
        CancellationToken cancellationToken)
    {
        var summaryData = await _dashboardService.GetSummaryAsync(query, cancellationToken);
        return Ok(summaryData);
    }
}
