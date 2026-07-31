using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/v1/tables/{tableName}/columns")]
public class ColumnsController(IColumnService columnService) : ControllerBase
{
    private readonly IColumnService _columnService = columnService;
    [HttpPost]
    public async Task<IActionResult> Create(string tableName, [FromBody] CreateColumnRequest request, CancellationToken cancellationToken)
    {
        var column = new Column(request.Name, DataTypeFactory.Create(request.DataType), request.IsNullable);
        var columnResult = await _columnService.CreateAsync(tableName, column, cancellationToken);

        var result = new ColumnResponse(columnResult.Name, columnResult.DataType, columnResult.IsNullable);
        return CreatedAtAction(nameof(GetByName), new { tableName }, result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(string tableName, GetColumnsRequest request, CancellationToken cancellationToken)
    {
        var columns = await _columnService.GetAllAsync(tableName, cancellationToken);
        var result = columns.Select(x => new ColumnResponse(x.Name, x.DataType, x.IsNullable));
        return Ok(result);
    }

    [HttpPut("{columnName}")]
    public async Task<IActionResult> Update(string tableName, string columnName, [FromBody] UpdateColumnRequest request, CancellationToken cancellationToken)
    {
        var column = new Column(request.Name, DataTypeFactory.Create(request.DataType), request.IsNullable);
        var columnResult = await _columnService.UpdateAsync(tableName, columnName, column, cancellationToken);

        var result = new ColumnResponse(columnResult.Name, columnResult.DataType, columnResult.IsNullable);
        return Ok(result);
    }

    [HttpDelete("{columnName}")]
    public async Task<IActionResult> Delete(string tableName, DeleteColumnRequest request, CancellationToken cancellationToken)
    {
        await _columnService.DeleteAsync(tableName, request.Name, cancellationToken);
        return NoContent();
    }

    [HttpGet("{columnName}")]
    public async Task<IActionResult> GetByName(string tableName, GetColumnRequest request, CancellationToken cancellationToken)
    {
        var column = await _columnService.GetAsync(tableName, request.Name, cancellationToken);
        if (column == null) return NotFound();

        var result = new ColumnResponse(column.Name, column.DataType, column.IsNullable);
        return Ok(result);
    }
}
