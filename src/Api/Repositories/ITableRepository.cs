public interface ITableRepository
{
    Task<List<Table>> GetAllAsync(CancellationToken cancellationToken);
    Task<Table?> GetAsync(int id, CancellationToken cancellationToken);
    Task<Table?> GetByNameAsync(int schemaId, string tableName, CancellationToken cancellationToken);

    Task<Table> CreateAsync(Table table, CancellationToken cancellationToken);
    Task<Table> UpdateAsync(int id, Table table, CancellationToken cancellationToken);
    Task DeleteAsync(int id, CancellationToken cancellationToken);
    Task SaveAsync(Table table, CancellationToken cancellationToken);

    // Columns
    Task<List<Column>> GetAllColumnsAsync(string tableName, CancellationToken cancellationToken);
    Task<Column?> GetColumnAsync(string tableName, string columnName, CancellationToken cancellationToken);

    // Rows
    Task<List<Row>> GetRowsAsync(string tableName, CancellationToken cancellationToken);
    Task<Row?> GetRowAsync(string tableName, int rowId, CancellationToken cancellationToken);
}
