public class ColumnService(ITableRepository tableRepository, ICatalogRepository catalogRepository) : IColumnService
{
    private readonly ITableRepository _tableRepository = tableRepository;
    private readonly ICatalogRepository _catalogRepository = catalogRepository;

    public async Task<Column> CreateAsync(string databaseName, string schemaName, string tableName, Column column, CancellationToken cancellationToken)
    {
        var tableId = await _catalogRepository.FindTableIdAsync(databaseName, schemaName, tableName, cancellationToken);
        if (tableId is null) throw new TableNotFoundException();

        var table = await _tableRepository.GetAsync(tableId.Value, cancellationToken);

        table!.AddColumn(column);

        await _tableRepository.SaveAsync(table, cancellationToken);
        return column;
    }

    public async Task DeleteAsync(string databaseName, string schemaName, string tableName, string columnName, CancellationToken cancellationToken)
    {
        var tableId = await _catalogRepository.FindTableIdAsync(databaseName, schemaName, tableName, cancellationToken);
        if (tableId is null) throw new TableNotFoundException();

        var table = await _tableRepository.GetAsync(tableId.Value, cancellationToken);
        table!.DropColumn(columnName);
        await _tableRepository.SaveAsync(table, cancellationToken);
        return;
    }

    public async Task<List<Column>> GetAllAsync(string databaseName, string schemaName, string tableName, CancellationToken cancellationToken)
    {
        return await _tableRepository.GetAllColumnsAsync(tableName, cancellationToken);
    }

    public async Task<Column?> GetAsync(string databaseName, string schemaName, string tableName, string columnName, CancellationToken cancellationToken)
    {
        return await _tableRepository.GetColumnAsync(tableName, columnName, cancellationToken);
    }

    public async Task<Column> UpdateAsync(string databaseName, string schemaName, string tableName, string columnName, Column column, CancellationToken cancellationToken)
    {
        var tableId = await _catalogRepository.FindTableIdAsync(databaseName, schemaName, tableName, cancellationToken);
        if (tableId is null) throw new TableNotFoundException();
        var table = await _tableRepository.GetAsync(tableId.Value, cancellationToken);

        table!.AlterColumn(columnName, column);
        await _tableRepository.SaveAsync(table, cancellationToken);
        return column;
    }
}
