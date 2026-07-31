using Microsoft.AspNetCore.Mvc;

public class TablesController(ITableService tableService) : ControllerBase
{
    private readonly ITableService _tableService = tableService;

    [HttpGet]
    public async Task<List<Table>> GetAll(CancellationToken cancellationToken)
    {
        var tables = await _tableService.GetAllAsync(cancellationToken);
        return tables;
    }

    [HttpPost]
    public async Task<Table> Create(Table table, CancellationToken cancellationToken)
    {
        var createdTable = await _tableService.CreateAsync(table, cancellationToken);
        return createdTable;
    }

    [HttpDelete("{tableName}")]
    public async Task Delete(string tableName, CancellationToken cancellationToken)
    {
        await _tableService.DeleteAsync(tableName, cancellationToken);
    }

    [HttpPut("{tableName}")]
    public async Task<Table> Update(string tableName, Table table, CancellationToken cancellationToken)
    {
        var updatedTable = await _tableService.UpdateAsync(tableName, table, cancellationToken);
        return updatedTable;
    }

    [HttpGet("{tableName}")]
    public async Task<IActionResult> Get(string tableName, CancellationToken cancellationToken)
    {
        var table = await _tableService.GetAsync(tableName, cancellationToken);
        if (table == null) return NotFound();
        return Ok(table);
    }
}
