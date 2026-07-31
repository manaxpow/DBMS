public interface ISchemaRepository
{
    Task<Schema?> GetByNameAsync(int databaseId, string schemaName, CancellationToken cancellationToken);
}
