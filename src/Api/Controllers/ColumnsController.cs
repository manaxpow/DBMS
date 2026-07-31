using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("databases/{DatabaseName}/schemas/{SchemaName}/tables/{TableName}/columns")]
public class ColumnsController(IColumnService columnService) : ControllerBase
{
    private readonly IColumnService _columnService = columnService;

    [HttpPost]
    public async Task<IActionResult> Create([FromRoute] TablePath tablePath, [FromBody] AddColumnRequest request, CancellationToken cancellationToken)
    {
        var column = new Column(request.Name, DataTypeFactory.Create(request.DataType), request.IsNullable);
        var columnResult = await _columnService.CreateAsync(tablePath, column, cancellationToken);

        var result = new ColumnResponse(columnResult.Id, columnResult.Name, columnResult.DataType, columnResult.IsNullable);
        return CreatedAtAction(nameof(GetByName), new { DatabaseName = tablePath.DatabaseName, SchemaName = tablePath.SchemaName, TableName = tablePath.TableName, columnName = columnResult.Name }, result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromRoute] TablePath tablePath, [FromQuery] GetColumnsRequest request, CancellationToken cancellationToken)
    {
        var pagedResult = await _columnService.GetAllAsync(tablePath, request, cancellationToken);
        var responseData = pagedResult.Data.Select(x => new ColumnResponse(x.Id, x.Name, x.DataType, x.IsNullable));
        var result = new PagedResponse<ColumnResponse>(responseData, pagedResult.TotalCount, pagedResult.Page, pagedResult.PageSize);
        return Ok(result);
    }

    [HttpPut("{columnName}")]
    public async Task<IActionResult> Update([FromRoute] TablePath tablePath, string columnName, [FromBody] AlterColumnRequest request, CancellationToken cancellationToken)
    {
        var column = new Column(request.Name, DataTypeFactory.Create(request.DataType), request.IsNullable);
        var columnResult = await _columnService.UpdateAsync(tablePath, columnName, column, cancellationToken);

        var result = new ColumnResponse(columnResult.Id, columnResult.Name, columnResult.DataType, columnResult.IsNullable);
        return Ok(result);
    }

    [HttpDelete("{columnName}")]
    public async Task<IActionResult> Delete([FromRoute] TablePath tablePath, string columnName, [FromQuery] DropColumnRequest request, CancellationToken cancellationToken)
    {
        await _columnService.DeleteAsync(tablePath, columnName, cancellationToken);
        return NoContent();
    }

    [HttpGet("{columnName}")]
    public async Task<IActionResult> GetByName([FromRoute] TablePath tablePath, string columnName, [FromQuery] GetColumnRequest request, CancellationToken cancellationToken)
    {
        var column = await _columnService.GetAsync(tablePath, columnName, cancellationToken);
        if (column == null) return NotFound();

        var result = new ColumnDetailResponse(column.Id, column.Name, column.DataType, column.IsNullable, null);
        return Ok(result);
    }
}


