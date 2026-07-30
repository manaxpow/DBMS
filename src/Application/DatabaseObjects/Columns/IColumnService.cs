public interface IColumnService
{
    Task<Column> CreateAsync(string tableName, Column column, CancellationToken cancellationToken);
    Task<Column> UpdateAsync(string tableName, string columnName, Column column, CancellationToken cancellationToken);
    Task DeleteAsync(string tableName, string columnName, CancellationToken cancellationToken);
    Task<List<Column>> GetAllAsync(string tableName, CancellationToken cancellationToken);
    Task<Column?> GetAsync(string tableName, string columnName, CancellationToken cancellationToken);
}
