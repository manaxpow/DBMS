using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("databases/{DatabaseName}/schemas/{SchemaName}/tables/{TableName}/rows")]
public sealed class RowsController(IRowService rowService) : ControllerBase
{
    private readonly IRowService _rowService = rowService;

    [HttpGet]
    public async Task<IActionResult> GetAll([FromRoute] TablePath tablePath, [FromQuery] GetRowsRequest request, CancellationToken cancellationToken)
    {
        var pagedResult = await _rowService.GetAllAsync(tablePath, request, cancellationToken);
        var responseData = pagedResult.Data.Select(x => new RowResponse(x.Id, x.Values.ToList()));
        var result = new PagedResponse<RowResponse>(responseData, pagedResult.TotalCount, pagedResult.Page, pagedResult.PageSize);
        return Ok(result);
    }

    [HttpGet("{rowId}")]
    public async Task<IActionResult> Get([FromRoute] TablePath tablePath, int rowId, CancellationToken cancellationToken)
    {
        var row = await _rowService.GetAsync(tablePath, rowId, cancellationToken);
        if (row == null) return NotFound();
        var result = new RowResponse(row.Id, row.Values.ToList());
        return Ok(result);
    }

    [HttpPut("{rowId}")]
    public async Task<IActionResult> Update([FromRoute] TablePath tablePath, int rowId, [FromBody] UpdateRowRequest request, CancellationToken cancellationToken)
    {

        var row = new Row(request.Values);
        var rowResult = await _rowService.UpdateAsync(tablePath, rowId, row, cancellationToken);

        var result = new RowResponse(rowResult.Id, rowResult.Values.ToList());
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromRoute] TablePath tablePath, [FromBody] CreateRowRequest request, CancellationToken cancellationToken)
    {
        var row = new Row(request.Values);
        var rowResult = await _rowService.CreateAsync(tablePath, row, cancellationToken);
        var result = new RowResponse(rowResult.Id, rowResult.Values.ToList());
        return CreatedAtAction(nameof(Get), new { DatabaseName = tablePath.DatabaseName, SchemaName = tablePath.SchemaName, TableName = tablePath.TableName, rowId = rowResult.Id }, result);
    }

    [HttpDelete("{rowId}")]
    public async Task<IActionResult> Delete([FromRoute] TablePath tablePath, int rowId, CancellationToken cancellationToken)
    {
        await _rowService.DeleteAsync(tablePath, rowId, cancellationToken);
        return NoContent();
    }

}


