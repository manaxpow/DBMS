public interface IColumnService
{
    Task<Column> CreateAsync(string databaseName, string schemaName, string tableName, Column column, CancellationToken cancellationToken);
    Task<Column> UpdateAsync(string databaseName, string schemaName, string tableName, string columnName, Column column, CancellationToken cancellationToken);
    Task DeleteAsync(string databaseName, string schemaName, string tableName, string columnName, CancellationToken cancellationToken);
    Task<List<Column>> GetAllAsync(string databaseName, string schemaName, string tableName, CancellationToken cancellationToken);
    Task<Column?> GetAsync(string databaseName, string schemaName, string tableName, string columnName, CancellationToken cancellationToken);
}
