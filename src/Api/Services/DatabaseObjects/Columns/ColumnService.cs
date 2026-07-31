public class ColumnService(ITableRepository tableRepository) : IColumnService
{
    private readonly ITableRepository _tableRepository = tableRepository;

    public async Task<Column> CreateAsync(TablePath tablePath, Column column, CancellationToken cancellationToken)
    {
        var table = await _tableRepository.GetByNameAsync(tablePath, cancellationToken);
        if (table == null) throw new Exception("Table not found");

        table.AddColumn(column);

        await _tableRepository.SaveAsync(tablePath.DatabaseName, tablePath.SchemaName, table, cancellationToken);
        return column;
    }

    public async Task DeleteAsync(TablePath tablePath, string columnName, CancellationToken cancellationToken)
    {
        var table = await _tableRepository.GetByNameAsync(tablePath, cancellationToken);
        if (table == null) throw new Exception("Table not found");

        table.DropColumn(columnName);
        await _tableRepository.SaveAsync(tablePath.DatabaseName, tablePath.SchemaName, table, cancellationToken);
    }

    public async Task<PagedResponse<Column>> GetAllAsync(TablePath tablePath, GetColumnsRequest request, CancellationToken cancellationToken)
    {
        return await _tableRepository.GetAllColumnsAsync(tablePath, request, cancellationToken);
    }

    public async Task<Column?> GetAsync(TablePath tablePath, string columnName, CancellationToken cancellationToken)
    {
        return await _tableRepository.GetColumnAsync(tablePath, columnName, cancellationToken);
    }

    public async Task<Column> UpdateAsync(TablePath tablePath, string columnName, Column column, CancellationToken cancellationToken)
    {
        var table = await _tableRepository.GetByNameAsync(tablePath, cancellationToken);
        if (table == null) throw new Exception("Table not found");

        table.AlterColumn(columnName, column);
        await _tableRepository.SaveAsync(tablePath.DatabaseName, tablePath.SchemaName, table, cancellationToken);
        return column;
    }
}


