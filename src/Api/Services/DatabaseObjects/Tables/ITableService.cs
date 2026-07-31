public interface ITableService
{
    Task<Table> CreateAsync(Table table, CancellationToken cancellationToken);
    Task<Table> UpdateAsync(string tableName, Table table, CancellationToken cancellationToken);
    Task DeleteAsync(string tableName, CancellationToken cancellationToken);
    Task<Table?> GetAsync(string tableName, CancellationToken cancellationToken);
    Task<List<Table>> GetAllAsync(CancellationToken cancellationToken);
}
