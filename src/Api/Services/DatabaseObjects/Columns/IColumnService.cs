public interface IColumnService
{
    Task<Column> CreateAsync(TablePath tablePath, Column column, CancellationToken cancellationToken);
    Task<Column> UpdateAsync(TablePath tablePath, string columnName, Column column, CancellationToken cancellationToken);
    Task DeleteAsync(TablePath tablePath, string columnName, CancellationToken cancellationToken);
    Task<PagedResponse<Column>> GetAllAsync(TablePath tablePath, GetColumnsRequest request, CancellationToken cancellationToken);
    Task<Column?> GetAsync(TablePath tablePath, string columnName, CancellationToken cancellationToken);
}


