using DBMS.Exceptions;

public class TableService : ITableService
{
    private readonly ITableRepository _tableRepository;
    private readonly IDatabaseRepository _databaseRepository;
    public TableService(ITableRepository tableRepository, IDatabaseRepository databaseRepository)
    {
        _tableRepository = tableRepository;
        _databaseRepository = databaseRepository;
    }

    public async Task<Table> CreateAsync(TablePath tablePath, Table table, CancellationToken cancellationToken)
    {
        await _tableRepository.CreateAsync(tablePath.DatabaseName, tablePath.SchemaName, table, cancellationToken);
        return table;
    }

    public async Task<Table> UpdateAsync(TablePath tablePath, Table table, CancellationToken cancellationToken)
    {
        var existingTable = await _tableRepository.GetByNameAsync(tablePath, cancellationToken);
        if (existingTable != null)
        {
            await _tableRepository.UpdateAsync(tablePath.DatabaseName, tablePath.SchemaName, existingTable.Id, table, cancellationToken);
        }
        return table;
    }

    public async Task DeleteAsync(TablePath tablePath, CancellationToken cancellationToken)
    {
        var existingTable = await _tableRepository.GetByNameAsync(tablePath, cancellationToken);
        if (existingTable != null)
        {
            await _tableRepository.DeleteAsync(tablePath, cancellationToken);
        }
    }

    public async Task<Table?> GetAsync(TablePath tablePath, CancellationToken cancellationToken)
    {
        var table = await _tableRepository.GetByNameAsync(tablePath, cancellationToken);
        return table;
    }

    public async Task<PagedResponse<Table>> GetAllAsync(string dbName, string schemaName, GetTablesRequest request, CancellationToken cancellationToken)
    {
        return await _tableRepository.GetAllAsync(dbName, schemaName, request, cancellationToken);
    }
}


