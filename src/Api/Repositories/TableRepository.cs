using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

public class TableRepository : ITableRepository
{
    private readonly IDatabaseRepository _databaseRepository;
    private int _nextId = 2; // Assume 1 is TestTable

    public TableRepository(IDatabaseRepository databaseRepository)
    {
        _databaseRepository = databaseRepository;
    }

    private async Task<Schema> GetDefaultSchemaAsync(CancellationToken cancellationToken)
    {
        var db = await _databaseRepository.GetByNameAsync("TestDB", cancellationToken);
        var schema = db?.GetSchema("public");
        if (schema == null) throw new Exception("Default database or schema not found");
        return schema;
    }

    public async Task<Table> CreateAsync(Table table, CancellationToken cancellationToken)
    {
        var schema = await GetDefaultSchemaAsync(cancellationToken);
        
        table.Id = _nextId++;
        schema.AddTable(table);
        
        return table;
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken)
    {
        var schema = await GetDefaultSchemaAsync(cancellationToken);
        var table = schema.Objects.OfType<Table>().FirstOrDefault(t => t.Id == id);
        if (table != null)
        {
            schema.DropTable(table.Name);
        }
    }

    public async Task<List<Table>> GetAllAsync(CancellationToken cancellationToken)
    {
        var schema = await GetDefaultSchemaAsync(cancellationToken);
        return schema.Objects.OfType<Table>().ToList();
    }

    public async Task<List<Column>> GetAllColumnsAsync(string tableName, CancellationToken cancellationToken)
    {
        var schema = await GetDefaultSchemaAsync(cancellationToken);
        var table = schema.GetTable(tableName);
        return table?.Columns.ToList() ?? new List<Column>();
    }

    public async Task<Table?> GetAsync(int id, CancellationToken cancellationToken)
    {
        var schema = await GetDefaultSchemaAsync(cancellationToken);
        return schema.Objects.OfType<Table>().FirstOrDefault(t => t.Id == id);
    }

    public async Task<Table?> GetByNameAsync(int schemaId, string tableName, CancellationToken cancellationToken)
    {
        var schema = await GetDefaultSchemaAsync(cancellationToken);
        return schema.GetTable(tableName);
    }

    public async Task<Column?> GetColumnAsync(string tableName, string columnName, CancellationToken cancellationToken)
    {
        var schema = await GetDefaultSchemaAsync(cancellationToken);
        var table = schema.GetTable(tableName);
        return table?.GetColumn(columnName);
    }

    public async Task<Row?> GetRowAsync(string tableName, int rowId, CancellationToken cancellationToken)
    {
        var schema = await GetDefaultSchemaAsync(cancellationToken);
        var table = schema.GetTable(tableName);
        return table?.Rows.FirstOrDefault(r => r.Id == rowId);
    }

    public async Task<List<Row>> GetRowsAsync(string tableName, CancellationToken cancellationToken)
    {
        var schema = await GetDefaultSchemaAsync(cancellationToken);
        var table = schema.GetTable(tableName);
        return table?.Rows.ToList() ?? new List<Row>();
    }

    public Task SaveAsync(Table table, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public async Task<Table> UpdateAsync(int id, Table table, CancellationToken cancellationToken)
    {
        var schema = await GetDefaultSchemaAsync(cancellationToken);
        var existingTable = schema.Objects.OfType<Table>().FirstOrDefault(t => t.Id == id);
        if (existingTable != null)
        {
            schema.DropTable(existingTable.Name);
            table.Id = id;
            schema.AddTable(table);
        }
        return table;
    }
}
