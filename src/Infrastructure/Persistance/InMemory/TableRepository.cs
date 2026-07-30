public class TableRepository : ITableRepository
{
    public Task<Table> CreateAsync(Table table, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(string tableName, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<List<Table>> GetAllAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<List<Column>> GetAllColumnsAsync(string tableName, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<Table> GetAsync(string tableName, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<Column?> GetColumnAsync(string tableName, string columnName, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task SaveAsync(Table table, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<Table> UpdateAsync(string tableName, Table table, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
