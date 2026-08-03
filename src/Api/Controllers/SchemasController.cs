using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/v1/databases/{DatabaseName}/schemas")]
public class SchemasController(ISchemaService schemaService) : ControllerBase
{
    private readonly ISchemaService _schemaService = schemaService;

    [HttpGet]
    public async Task<IActionResult> GetAll(string DatabaseName, CancellationToken cancellationToken)
    {
        var schemas = await _schemaService.GetAllAsync(DatabaseName, cancellationToken);
        return Ok(schemas);
    }

    [HttpGet("{SchemaName}")]
    public async Task<IActionResult> Get(string DatabaseName, string SchemaName, CancellationToken cancellationToken)
    {
        var schema = await _schemaService.GetByNameAsync(DatabaseName, SchemaName, cancellationToken);
        return Ok(schema);
    }

    [HttpPost]
    public async Task<IActionResult> Create(string DatabaseName, [FromBody] CreateSchemaRequest request, CancellationToken cancellationToken)
    {
        var schema = await _schemaService.CreateAsync(DatabaseName, request.Name, cancellationToken);
        return Ok(schema);
    }

    [HttpDelete("{SchemaName}")]
    public async Task<IActionResult> Delete(string DatabaseName, string SchemaName, CancellationToken cancellationToken)
    {
        await _schemaService.DeleteAsync(DatabaseName, SchemaName, cancellationToken);
        return NoContent();
    }

    [HttpPut("{SchemaName}")]
    public async Task<IActionResult> Update(string DatabaseName, string SchemaName, [FromBody] UpdateSchemaRequest request, CancellationToken cancellationToken)
    {
        var schema = await _schemaService.UpdateAsync(DatabaseName, SchemaName, request.Name, cancellationToken);
        return Ok(schema);
    }

}
