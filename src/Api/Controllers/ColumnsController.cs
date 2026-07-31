using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("databases/{dbName}/schemas/{schemaName}/tables/{tableName}/columns")]
public class ColumnsController(IColumnService columnService) : ControllerBase
{
    private readonly IColumnService _columnService = columnService;

    [HttpPost]
    public async Task<IActionResult> Create(string dbName, string schemaName, string tableName, [FromBody] AddColumnRequest request, CancellationToken cancellationToken)
    {
        var column = new Column(request.Name, DataTypeFactory.Create(request.DataType), request.IsNullable);
        var columnResult = await _columnService.CreateAsync(dbName, schemaName, tableName, column, cancellationToken);

        var result = new ColumnResponse(columnResult.Id, columnResult.Name, columnResult.DataType, columnResult.IsNullable);
        return CreatedAtAction(nameof(GetByName), new { dbName, schemaName, tableName, columnName = columnResult.Name }, result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(string dbName, string schemaName, string tableName, [FromQuery] GetColumnsRequest request, CancellationToken cancellationToken)
    {
        var columns = await _columnService.GetAllAsync(dbName, schemaName, tableName, cancellationToken);
        var result = columns.Select(x => new ColumnResponse(x.Id, x.Name, x.DataType, x.IsNullable));
        return Ok(result);
    }

    [HttpPut("{columnName}")]
    public async Task<IActionResult> Update(string dbName, string schemaName, string tableName, string columnName, [FromBody] AlterColumnRequest request, CancellationToken cancellationToken)
    {
        var column = new Column(request.Name, DataTypeFactory.Create(request.DataType), request.IsNullable);
        var columnResult = await _columnService.UpdateAsync(dbName, schemaName, tableName, columnName, column, cancellationToken);

        var result = new ColumnResponse(columnResult.Id, columnResult.Name, columnResult.DataType, columnResult.IsNullable);
        return Ok(result);
    }

    [HttpDelete("{columnName}")]
    public async Task<IActionResult> Delete(string dbName, string schemaName, string tableName, string columnName, [FromQuery] DropColumnRequest request, CancellationToken cancellationToken)
    {
        await _columnService.DeleteAsync(dbName, schemaName, tableName, columnName, cancellationToken);
        return NoContent();
    }

    [HttpGet("{columnName}")]
    public async Task<IActionResult> GetByName(string dbName, string schemaName, string tableName, string columnName, [FromQuery] GetColumnRequest request, CancellationToken cancellationToken)
    {
        var column = await _columnService.GetAsync(dbName, schemaName, tableName, columnName, cancellationToken);
        if (column == null) return NotFound();

        var result = new ColumnDetailResponse(column.Id, column.Name, column.DataType, column.IsNullable, null);
        return Ok(result);
    }
}
