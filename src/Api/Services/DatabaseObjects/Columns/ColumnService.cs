public class ColumnService(ITableRepository tableRepository) : IColumnService
{
    private readonly ITableRepository _tableRepository = tableRepository;

    public async Task<Column> CreateAsync(string tableName, Column column, CancellationToken cancellationToken)
    {
        var table = await _tableRepository.GetAsync(tableName, cancellationToken);

        table.AddColumn(column);

        await _tableRepository.SaveAsync(table, cancellationToken);
        return column;
    }

    public async Task DeleteAsync(string tableName, string columnName, CancellationToken cancellationToken)
    {
        var table = await _tableRepository.GetAsync(tableName, cancellationToken);
        table.DropColumn(columnName);
        await _tableRepository.SaveAsync(table, cancellationToken);
        return;
    }

    public async Task<List<Column>> GetAllAsync(string tableName, CancellationToken cancellationToken)
    {
        return await _tableRepository.GetAllColumnsAsync(tableName, cancellationToken);
    }

    public async Task<Column?> GetAsync(string tableName, string columnName, CancellationToken cancellationToken)
    {
        return await _tableRepository.GetColumnAsync(tableName, columnName, cancellationToken);
    }

    public async Task<Column> UpdateAsync(string tableName, string columnName, Column column, CancellationToken cancellationToken)
    {
        var table = await _tableRepository.GetAsync(tableName, cancellationToken);
        table.AlterColumn(columnName, column);
        await _tableRepository.SaveAsync(table, cancellationToken);
        return column;
    }
}
