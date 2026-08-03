public interface ISchemaRepository
{
    Task<Schema?> GetByNameAsync(string databaseName, string schemaName, CancellationToken cancellationToken);

    Task<IEnumerable<Schema>> GetAllAsync(string databaseName, CancellationToken cancellationToken);

    Task<Schema> CreateAsync(string databaseName, Schema schema, CancellationToken cancellationToken);

    Task<Schema> UpdateAsync(string databaseName, string schemaName, string newSchemaName, CancellationToken cancellationToken);

    Task DeleteAsync(string databaseName, string schemaName, CancellationToken cancellationToken);
}
