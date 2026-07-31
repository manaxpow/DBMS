using System.Text.Json;

public class TableRepository : ITableRepository
{
    private readonly string _filePath;
    private readonly JsonSerializerOptions _serializerOptions;
    private readonly SemaphoreSlim _lock = new(1, 1);

    public TableRepository(string filePath)
    {
        _filePath = filePath;
        _serializerOptions = new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNameCaseInsensitive = true
        };
    }
    public async Task<Table> CreateAsync(Table table, CancellationToken cancellationToken)
    {
        await _lock.WaitAsync(cancellationToken);
        try
        {
            var tables = await ReadTableAsync(cancellationToken);

            var existingTable = tables.FirstOrDefault(t => t.Name == table.Name);
            if (existingTable != null)
            {
                throw new TableAlreadyExistsException(table.Name);
            }
            tables.Add(table);
            await File.WriteAllTextAsync(_filePath, JsonSerializer.Serialize(tables, _serializerOptions), cancellationToken);

            return table;
        }
        finally
        {
            _lock.Release();
        }
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

    public Task<Row?> GetRowAsync(string tableName, int rowId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<List<Row>> GetRowsAsync(string tableName, CancellationToken cancellationToken)
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

    private async Task<List<Table>> ReadTableAsync(CancellationToken cancellationToken)
    {
        if (File.Exists(_filePath))
        {
            var json = await File.ReadAllTextAsync(_filePath, cancellationToken);
            return JsonSerializer.Deserialize<List<Table>>(json, _serializerOptions) ?? new List<Table>();
        }
        return new List<Table>();
    }

    private async Task WriteTableAsync(List<Table> tables, CancellationToken cancellationToken)
    {
        var directory = Path.GetDirectoryName(_filePath);
        if (!string.IsNullOrWhiteSpace(directory))
        {
            Directory.CreateDirectory(directory);
        }

        var json = JsonSerializer.Serialize(tables, _serializerOptions);
        await File.WriteAllTextAsync(_filePath, json, cancellationToken);
    }
}
