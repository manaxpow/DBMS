public interface ITableRepository
{
    Task<List<Table>> GetAllAsync(CancellationToken cancellationToken);
    Task<Table> GetAsync(string tableName, CancellationToken cancellationToken);
    Task<Table> CreateAsync(Table table, CancellationToken cancellationToken);
    Task<Table> UpdateAsync(string tableName, Table table, CancellationToken cancellationToken);
    Task DeleteAsync(string tableName, CancellationToken cancellationToken);
    Task SaveAsync(Table table, CancellationToken cancellationToken);

    // Columns
    Task<List<Column>> GetAllColumnsAsync(string tableName, CancellationToken cancellationToken);
    Task<Column?> GetColumnAsync(string tableName, string columnName, CancellationToken cancellationToken);

    // Rows
    Task<List<Row>> GetRowsAsync(string tableName, CancellationToken cancellationToken);
    Task<Row?> GetRowAsync(string tableName, int rowId, CancellationToken cancellationToken);
}
