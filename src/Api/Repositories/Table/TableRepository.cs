public class TableRepository : ITableRepository
{
    private readonly IDatabaseRepository _databaseRepository;
    private int _nextId = 2;

    public TableRepository(IDatabaseRepository databaseRepository)
    {
        _databaseRepository = databaseRepository;
    }

    private async Task<Schema> GetSchemaAsync(string dbName, string schemaName, CancellationToken cancellationToken)
    {
        var db = await _databaseRepository.GetByNameAsync(dbName, cancellationToken);
        var schema = db?.GetSchema(schemaName);
        if (schema == null) throw new KeyNotFoundException($"Database '{dbName}' or schema '{schemaName}' not found");
        return schema;
    }

    public async Task<Table> CreateAsync(string dbName, string schemaName, Table table, CancellationToken cancellationToken)
    {
        var schema = await GetSchemaAsync(dbName, schemaName, cancellationToken);

        table.Id = _nextId++;
        schema.AddTable(table);

        return table;
    }

    public async Task DeleteAsync(TablePath tablePath, CancellationToken cancellationToken)
    {
        var schema = await GetSchemaAsync(tablePath.DatabaseName, tablePath.SchemaName, cancellationToken);
        var table = schema.Objects.OfType<Table>().FirstOrDefault(t => t.Name == tablePath.TableName);
        if (table != null)
        {
            schema.DropTable(table.Name);
        }
    }

    public async Task<PagedResponse<Table>> GetAllAsync(string dbName, string schemaName, GetTablesRequest request, CancellationToken cancellationToken)
    {
        var schema = await GetSchemaAsync(dbName, schemaName, cancellationToken);
        var query = schema.Objects.OfType<Table>().AsEnumerable();

        if (!string.IsNullOrWhiteSpace(request.Name))
            query = query.Where(t => t.Name.Contains(request.Name, StringComparison.OrdinalIgnoreCase));

        if (request.HasRows.HasValue)
            query = query.Where(t => (t.Rows.Count > 0) == request.HasRows.Value);

        if (request.Partitioned.HasValue)
            query = query.Where(t => (t.Partitions.Count > 0) == request.Partitioned.Value);

        if (!string.IsNullOrWhiteSpace(request.Sort))
        {
            query = request.Sort.ToLower() switch
            {
                "name" => query.OrderBy(t => t.Name),
                "-name" => query.OrderByDescending(t => t.Name),
                _ => query.OrderBy(t => t.Id)
            };
        }
        else
        {
            query = query.OrderBy(t => t.Id);
        }

        var totalCount = query.Count();
        var page = request.Page ?? 1;
        var pageSize = request.PageSize ?? 10;
        var tables = query.Skip((page - 1) * pageSize).Take(pageSize).ToList();

        return new PagedResponse<Table>(tables, totalCount, page, pageSize);
    }

    public async Task<PagedResponse<Column>> GetAllColumnsAsync(TablePath tablePath, GetColumnsRequest request, CancellationToken cancellationToken)
    {
        var schema = await GetSchemaAsync(tablePath.DatabaseName, tablePath.SchemaName, cancellationToken);
        var table = schema.GetTable(tablePath.TableName);
        if (table == null) return new PagedResponse<Column>(new List<Column>(), 0, request.Page ?? 1, request.PageSize ?? 10);

        var query = table.Columns.AsEnumerable();

        if (!string.IsNullOrWhiteSpace(request.Name))
            query = query.Where(c => c.Name.Contains(request.Name, StringComparison.OrdinalIgnoreCase));

        if (!string.IsNullOrWhiteSpace(request.DataType))
            query = query.Where(c => c.DataType.Name.Equals(request.DataType, StringComparison.OrdinalIgnoreCase));

        if (request.Nullable.HasValue)
            query = query.Where(c => c.IsNullable == request.Nullable.Value);

        if (!string.IsNullOrWhiteSpace(request.Sort))
        {
            query = request.Sort.ToLower() switch
            {
                "name" => query.OrderBy(c => c.Name),
                "-name" => query.OrderByDescending(c => c.Name),
                "datatype" => query.OrderBy(c => c.DataType.Name),
                "-datatype" => query.OrderByDescending(c => c.DataType.Name),
                _ => query.OrderBy(c => c.Id)
            };
        }
        else
        {
            query = query.OrderBy(c => c.Id);
        }

        var totalCount = query.Count();
        var page = request.Page ?? 1;
        var pageSize = request.PageSize ?? 10;
        var columns = query.Skip((page - 1) * pageSize).Take(pageSize).ToList();

        return new PagedResponse<Column>(columns, totalCount, page, pageSize);
    }

    public async Task<Table?> GetAsync(string dbName, string schemaName, int id, CancellationToken cancellationToken)
    {
        var schema = await GetSchemaAsync(dbName, schemaName, cancellationToken);
        return schema.Objects.OfType<Table>().FirstOrDefault(t => t.Id == id);
    }

    public async Task<Table?> GetByNameAsync(TablePath tablePath, CancellationToken cancellationToken)
    {
        var schema = await GetSchemaAsync(tablePath.DatabaseName, tablePath.SchemaName, cancellationToken);
        return schema.GetTable(tablePath.TableName);
    }

    public async Task<Column?> GetColumnAsync(TablePath tablePath, string columnName, CancellationToken cancellationToken)
    {
        var schema = await GetSchemaAsync(tablePath.DatabaseName, tablePath.SchemaName, cancellationToken);
        var table = schema.GetTable(tablePath.TableName);
        return table?.GetColumn(columnName);
    }

    public async Task<Row?> GetRowAsync(TablePath tablePath, int rowId, CancellationToken cancellationToken)
    {
        var schema = await GetSchemaAsync(tablePath.DatabaseName, tablePath.SchemaName, cancellationToken);
        var table = schema.GetTable(tablePath.TableName);
        return table?.Rows.FirstOrDefault(r => r.Id == rowId);
    }

    public async Task<PagedResponse<Row>> GetRowsAsync(TablePath tablePath, GetRowsRequest request, CancellationToken cancellationToken)
    {
        var schema = await GetSchemaAsync(tablePath.DatabaseName, tablePath.SchemaName, cancellationToken);
        var table = schema.GetTable(tablePath.TableName);
        var query = (table?.Rows ?? Enumerable.Empty<Row>()).AsEnumerable();

        if (!string.IsNullOrWhiteSpace(request.Sort))
        {
            query = request.Sort.ToLower() switch
            {
                "id" => query.OrderBy(r => r.Id),
                "-id" => query.OrderByDescending(r => r.Id),
                _ => query.OrderBy(r => r.Id)
            };
        }
        else
        {
            query = query.OrderBy(r => r.Id);
        }

        var totalCount = query.Count();
        var page = request.Page ?? 1;
        var pageSize = request.PageSize ?? 10;
        var rows = query.Skip((page - 1) * pageSize).Take(pageSize).ToList();

        return new PagedResponse<Row>(rows, totalCount, page, pageSize);
    }

    public Task SaveAsync(string dbName, string schemaName, Table table, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public async Task<Table> UpdateAsync(string dbName, string schemaName, int id, Table table, CancellationToken cancellationToken)
    {
        var schema = await GetSchemaAsync(dbName, schemaName, cancellationToken);
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



