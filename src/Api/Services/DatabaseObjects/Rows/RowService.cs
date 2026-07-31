public class RowService(ITableRepository tableRepository) : IRowService
{
    private readonly ITableRepository _tableRepository = tableRepository;

    public async Task<List<Row>> GetAllAsync(string tableName, CancellationToken cancellationToken)
    {
        var rows = await _tableRepository.GetRowsAsync(tableName, cancellationToken);
        return rows;
    }

    public async Task<Row?> GetAsync(string tableName, int rowId, CancellationToken cancellationToken)
    {
        var row = await _tableRepository.GetRowAsync(tableName, rowId, cancellationToken);
        return row;
    }

    public async Task<Row> CreateAsync(string tableName, Row row, CancellationToken cancellationToken)
    {
        var table = await _tableRepository.GetByNameAsync(0, tableName, cancellationToken);
        if (table == null) throw new Exception("Table not found");
        table.InsertRow(row);
        await _tableRepository.SaveAsync(table, cancellationToken);
        return row;
    }

    public async Task<Row> UpdateAsync(string tableName, int rowId, Row row, CancellationToken cancellationToken)
    {
        var table = await _tableRepository.GetByNameAsync(0, tableName, cancellationToken);
        if (table == null) throw new Exception("Table not found");
        table.UpdateRow(rowId, row);
        await _tableRepository.SaveAsync(table, cancellationToken);
        return row;
    }

    public async Task DeleteAsync(string tableName, int rowId, CancellationToken cancellationToken)
    {
        var table = await _tableRepository.GetByNameAsync(0, tableName, cancellationToken);
        if (table == null) throw new Exception("Table not found");
        table.DeleteRow(rowId);
        await _tableRepository.SaveAsync(table, cancellationToken);
    }
}
