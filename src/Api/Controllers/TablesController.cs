using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("databases/{DatabaseName}/schemas/{SchemaName}/tables")]
public class TablesController(ITableService tableService) : ControllerBase
{
    private readonly ITableService _tableService = tableService;

    [HttpGet]
    public async Task<PagedResponse<TableResponse>> GetAll(string DatabaseName, string SchemaName, [FromQuery] GetTablesRequest request, CancellationToken cancellationToken)
    {
        var pagedResult = await _tableService.GetAllAsync(DatabaseName, SchemaName, request, cancellationToken);
        var responseData = pagedResult.Data.Select(t => new TableResponse(t.Id, t.Name)).ToList();
        return new PagedResponse<TableResponse>(responseData, pagedResult.TotalCount, pagedResult.Page, pagedResult.PageSize);
    }

    [HttpPost]
    public async Task<TableResponse> Create(string DatabaseName, string SchemaName, CreateTableRequest request, CancellationToken cancellationToken)
    {
        var tablePath = new TablePath(DatabaseName, SchemaName, request.Name);
        var table = new Table(request.Name);
        var createdTable = await _tableService.CreateAsync(tablePath, table, cancellationToken);
        return new TableResponse(createdTable.Id, createdTable.Name);
    }

    [HttpDelete("{TableName}")]
    public async Task Delete([FromRoute] TablePath tablePath, CancellationToken cancellationToken)
    {
        await _tableService.DeleteAsync(tablePath, cancellationToken);
    }

    [HttpPut("{TableName}")]
    public async Task<TableResponse> Update([FromRoute] TablePath tablePath, UpdateTableRequest request, CancellationToken cancellationToken)
    {
        var table = new Table(request.Name);
        var updatedTable = await _tableService.UpdateAsync(tablePath, table, cancellationToken);
        return new TableResponse(updatedTable.Id, updatedTable.Name);
    }

    [HttpGet("{TableName}")]
    public async Task<IActionResult> Get([FromRoute] TablePath tablePath, CancellationToken cancellationToken)
    {
        var table = await _tableService.GetAsync(tablePath, cancellationToken);
        if (table == null) return NotFound();
        
        var columns = table.Columns.Select(c => new ColumnResponse(c.Id, c.Name, c.DataType, c.IsNullable)).ToList();
        var response = new TableDetailResponse(table.Id, table.Name, columns);
        return Ok(response);
    }
}


