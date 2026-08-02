using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/v1/reports")]
[Authorize]
public class ReportsController(IReportService reportService) : ControllerBase
{
    private readonly IReportService _reportService = reportService;

    [Authorize(Roles = "Admin")]
    [HttpGet("revenue")]
    public async Task<ActionResult<RevenueReportResponse>> GetRevenueReport(
        [FromQuery] GetRevenueReportQuery query,
        CancellationToken cancellationToken = default)
    {
        var report = await _reportService.GetRevenueReportAsync(query, cancellationToken);
        return Ok(report);
    }
}
