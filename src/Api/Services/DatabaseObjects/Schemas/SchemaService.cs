public class SchemaService(ISchemaRepository schemaRepository) : ISchemaService
{
    private readonly ISchemaRepository _schemaRepository = schemaRepository;
    public async Task<Schema> CreateAsync(string databaseName, string schemaName, CancellationToken cancellationToken)
    {
        var schema = new Schema(schemaName);
        return await _schemaRepository.CreateAsync(databaseName, schema, cancellationToken);
    }

    public async Task DeleteAsync(string databaseName, string schemaName, CancellationToken cancellationToken)
    {
        await _schemaRepository.DeleteAsync(databaseName, schemaName, cancellationToken);
    }

    public async Task<IEnumerable<Schema>> GetAllAsync(string databaseName, CancellationToken cancellationToken)
    {
        return await _schemaRepository.GetAllAsync(databaseName, cancellationToken);
    }

    public async Task<Schema?> GetByNameAsync(string databaseName, string schemaName, CancellationToken cancellationToken)
    {
        return await _schemaRepository.GetByNameAsync(databaseName, schemaName, cancellationToken);
    }

    public async Task<Schema> UpdateAsync(string databaseName, string schemaName, string newSchemaName, CancellationToken cancellationToken)
    {
        return await _schemaRepository.UpdateAsync(databaseName, schemaName, newSchemaName, cancellationToken);
    }
}
