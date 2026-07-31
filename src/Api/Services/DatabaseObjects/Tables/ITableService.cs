public interface ITableService
{
    Task<Table> CreateAsync(TablePath tablePath, Table table, CancellationToken cancellationToken);
    Task<Table> UpdateAsync(TablePath tablePath, Table table, CancellationToken cancellationToken);
    Task DeleteAsync(TablePath tablePath, CancellationToken cancellationToken);
    Task<Table?> GetAsync(TablePath tablePath, CancellationToken cancellationToken);
    Task<PagedResponse<Table>> GetAllAsync(string dbName, string schemaName, GetTablesRequest request, CancellationToken cancellationToken);
}


