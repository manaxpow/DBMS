public interface ITableRepository
{
    Task<PagedResponse<Table>> GetAllAsync(string dbName, string schemaName, GetTablesRequest request, CancellationToken cancellationToken);
    Task<Table?> GetAsync(string dbName, string schemaName, int id, CancellationToken cancellationToken);
    Task<Table?> GetByNameAsync(TablePath tablePath, CancellationToken cancellationToken);

    Task<Table> CreateAsync(string dbName, string schemaName, Table table, CancellationToken cancellationToken);
    Task<Table> UpdateAsync(string dbName, string schemaName, int id, Table table, CancellationToken cancellationToken);
    Task DeleteAsync(TablePath tablePath, CancellationToken cancellationToken);
    Task SaveAsync(string dbName, string schemaName, Table table, CancellationToken cancellationToken);

    // Columns
    Task<PagedResponse<Column>> GetAllColumnsAsync(TablePath tablePath, GetColumnsRequest request, CancellationToken cancellationToken);
    Task<Column?> GetColumnAsync(TablePath tablePath, string columnName, CancellationToken cancellationToken);

    // Rows
    Task<PagedResponse<Row>> GetRowsAsync(TablePath tablePath, GetRowsRequest request, CancellationToken cancellationToken);
    Task<Row?> GetRowAsync(TablePath tablePath, int rowId, CancellationToken cancellationToken);
}



