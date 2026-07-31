using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/v1/tables/{tableName}/rows")]
public sealed class RowsController(IRowService rowService) : ControllerBase
{
    private readonly IRowService _rowService = rowService;

    [HttpGet]
    public async Task<IActionResult> GetAll(string tableName, CancellationToken cancellationToken)
    {
        var rows = await _rowService.GetAllAsync(tableName, cancellationToken);
        return Ok(rows);
    }

    [HttpGet("{rowId}")]
    public async Task<IActionResult> Get(string tableName, int rowId, CancellationToken cancellationToken)
    {
        var row = await _rowService.GetAsync(tableName, rowId, cancellationToken);
        return Ok(row);
    }

    [HttpPut("{rowId}")]
    public async Task<IActionResult> Update(string tableName, int rowId, [FromBody] CreateRowRequest request, CancellationToken cancellationToken)
    {
        var row = await _rowService.UpdateAsync(tableName, rowId, request, cancellationToken);
        return Ok(row);
    }

    [HttpPost]
    public async Task<IActionResult> Create(string tableName, [FromBody] CreateRowRequest request, CancellationToken cancellationToken)
    {
        var row = await _rowService.CreateAsync(tableName, request, cancellationToken);
        return CreatedAtAction(nameof(Get), new { tableName }, row);
    }

    [HttpDelete("{rowId}")]
    public async Task<IActionResult> Delete(string tableName, int rowId, CancellationToken cancellationToken)
    {
        await _rowService.DeleteAsync(tableName, rowId, cancellationToken);
        return NoContent();
    }

}
