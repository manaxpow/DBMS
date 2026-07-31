public class TableService : ITableService
{
    private readonly ITableRepository _tableRepository;
    private readonly IColumnService _columnService;
    private readonly IRowService _rowService;

    public TableService(ITableRepository tableRepository, IColumnService columnService, IRowService rowService)
    {
        _tableRepository = tableRepository;
        _columnService = columnService;
        _rowService = rowService;
    }

    public async Task<Table> CreateAsync(Table table, CancellationToken cancellationToken)
    {
        await _tableRepository.CreateAsync(table, cancellationToken);
        return table;
    }

    public async Task<Table> UpdateAsync(string tableName, Table table, CancellationToken cancellationToken)
    {
        var existingTable = await _tableRepository.GetByNameAsync(0, tableName, cancellationToken);
        if (existingTable != null)
        {
            await _tableRepository.UpdateAsync(existingTable.Id, table, cancellationToken);
        }
        return table;
    }

    public async Task DeleteAsync(string tableName, CancellationToken cancellationToken)
    {
        var existingTable = await _tableRepository.GetByNameAsync(0, tableName, cancellationToken);
        if (existingTable != null)
        {
            await _tableRepository.DeleteAsync(existingTable.Id, cancellationToken);
        }
    }

    public async Task<Table?> GetAsync(string tableName, CancellationToken cancellationToken)
    {
        var table = await _tableRepository.GetByNameAsync(0, tableName, cancellationToken);
        return table;
    }

    public async Task<List<Table>> GetAllAsync(CancellationToken cancellationToken)
    {
        var tables = await _tableRepository.GetAllAsync(cancellationToken);
        return tables;
    }
}
