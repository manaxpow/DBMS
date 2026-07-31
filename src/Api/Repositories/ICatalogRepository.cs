public interface ICatalogRepository
{
    Task<int?> FindTableIdAsync(
        string databaseName,
        string schemaName,
        string tableName,
        CancellationToken cancellationToken);
}
