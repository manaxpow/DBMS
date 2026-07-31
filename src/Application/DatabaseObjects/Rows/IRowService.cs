public interface IRowService
{
    Task<List<Row>> GetAllAsync(string tableName, CancellationToken cancellationToken);
    Task<Row?> GetAsync(string tableName, int rowId, CancellationToken cancellationToken);
    Task<Row> CreateAsync(string tableName, Row row, CancellationToken cancellationToken);
    Task<Row> UpdateAsync(string tableName, int rowId, Row row, CancellationToken cancellationToken);
    Task DeleteAsync(string tableName, int rowId, CancellationToken cancellationToken);
}
