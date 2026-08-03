public interface ISchemaService
{
    Task<IEnumerable<Schema>> GetAllAsync(string databaseName, CancellationToken cancellationToken);
    Task<Schema?> GetByNameAsync(string databaseName, string schemaName, CancellationToken cancellationToken);
    Task<Schema> CreateAsync(string databaseName, string schemaName, CancellationToken cancellationToken);
    Task DeleteAsync(string databaseName, string schemaName, CancellationToken cancellationToken);
    Task<Schema> UpdateAsync(string databaseName, string schemaName, string newSchemaName, CancellationToken cancellationToken);
}
