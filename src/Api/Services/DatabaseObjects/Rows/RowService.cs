public class RowService(ITableRepository tableRepository) : IRowService
{
    private readonly ITableRepository _tableRepository = tableRepository;

    public async Task<PagedResponse<Row>> GetAllAsync(TablePath tablePath, GetRowsRequest request, CancellationToken cancellationToken)
    {
        return await _tableRepository.GetRowsAsync(tablePath, request, cancellationToken);
    }

    public async Task<Row?> GetAsync(TablePath tablePath, int rowId, CancellationToken cancellationToken)
    {
        var row = await _tableRepository.GetRowAsync(tablePath, rowId, cancellationToken);
        return row;
    }

    public async Task<Row> CreateAsync(TablePath tablePath, Row row, CancellationToken cancellationToken)
    {
        var table = await _tableRepository.GetByNameAsync(tablePath, cancellationToken);
        if (table == null) throw new Exception("Table not found");
        table.InsertRow(row);
        await _tableRepository.SaveAsync(tablePath.DatabaseName, tablePath.SchemaName, table, cancellationToken);
        return row;
    }

    public async Task<Row> UpdateAsync(TablePath tablePath, int rowId, Row row, CancellationToken cancellationToken)
    {
        var table = await _tableRepository.GetByNameAsync(tablePath, cancellationToken);
        if (table == null) throw new Exception("Table not found");
        table.UpdateRow(rowId, row);
        await _tableRepository.SaveAsync(tablePath.DatabaseName, tablePath.SchemaName, table, cancellationToken);
        return row;
    }

    public async Task DeleteAsync(TablePath tablePath, int rowId, CancellationToken cancellationToken)
    {
        var table = await _tableRepository.GetByNameAsync(tablePath, cancellationToken);
        if (table == null) throw new Exception("Table not found");
        table.DeleteRow(rowId);
        await _tableRepository.SaveAsync(tablePath.DatabaseName, tablePath.SchemaName, table, cancellationToken);
    }
}


