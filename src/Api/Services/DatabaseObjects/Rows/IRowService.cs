public interface IRowService
{
    Task<PagedResponse<Row>> GetAllAsync(TablePath tablePath, GetRowsRequest request, CancellationToken cancellationToken);
    Task<Row?> GetAsync(TablePath tablePath, int rowId, CancellationToken cancellationToken);
    Task<Row> CreateAsync(TablePath tablePath, Row row, CancellationToken cancellationToken);
    Task<Row> UpdateAsync(TablePath tablePath, int rowId, Row row, CancellationToken cancellationToken);
    Task DeleteAsync(TablePath tablePath, int rowId, CancellationToken cancellationToken);
}


