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
        var result = rows.Select(x => new RowResponse(x.Id, x.Values.ToList()));
        return Ok(result);
    }

    [HttpGet("{rowId}")]
    public async Task<IActionResult> Get(string tableName, int rowId, CancellationToken cancellationToken)
    {
        var row = await _rowService.GetAsync(tableName, rowId, cancellationToken);
        if (row == null) return NotFound();
        var result = new RowResponse(row.Id, row.Values.ToList());
        return Ok(result);
    }

    [HttpPut("{rowId}")]
    public async Task<IActionResult> Update(string tableName, int rowId, [FromBody] UpdateRowRequest request, CancellationToken cancellationToken)
    {

        var row = new Row(request.Values);
        var rowResult = await _rowService.UpdateAsync(tableName, rowId, row, cancellationToken);

        var result = new RowResponse(rowResult.Id, rowResult.Values.ToList());
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create(string tableName, [FromBody] CreateRowRequest request, CancellationToken cancellationToken)
    {
        var row = new Row(request.Values);
        var rowResult = await _rowService.CreateAsync(tableName, row, cancellationToken);
        var result = new RowResponse(rowResult.Id, rowResult.Values.ToList());
        return CreatedAtAction(nameof(Get), new { tableName, rowId = rowResult.Id }, result);
    }

    [HttpDelete("{rowId}")]
    public async Task<IActionResult> Delete(string tableName, int rowId, CancellationToken cancellationToken)
    {
        await _rowService.DeleteAsync(tableName, rowId, cancellationToken);
        return NoContent();
    }

}
