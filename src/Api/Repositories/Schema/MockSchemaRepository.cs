using System.Threading;
using System.Threading.Tasks;

public sealed class MockSchemaRepository : ISchemaRepository
{
    private readonly IDatabaseRepository _databaseRepository;

    public MockSchemaRepository(IDatabaseRepository databaseRepository)
    {
        _databaseRepository = databaseRepository;
    }

    public Task<Schema> CreateAsync(string databaseName, Schema schema, CancellationToken cancellationToken)
    {
        var db = _databaseRepository.GetByNameAsync(databaseName, cancellationToken).Result;
        db?.AddSchema(schema);
        return Task.FromResult(schema);
    }

    public Task DeleteAsync(string databaseName, string schemaName, CancellationToken cancellationToken)
    {
        var db = _databaseRepository.GetByNameAsync(databaseName, cancellationToken).Result;
        db?.DropSchema(schemaName);
        return Task.CompletedTask;
    }

    public Task<IEnumerable<Schema>> GetAllAsync(string databaseName, CancellationToken cancellationToken)
    {
        var db = _databaseRepository.GetByNameAsync(databaseName, cancellationToken).Result;
        return Task.FromResult(db?.Schemas ?? Enumerable.Empty<Schema>());
    }


    public async Task<Schema?> GetByNameAsync(int databaseId, string schemaName, CancellationToken cancellationToken)
    {
        var db = await _databaseRepository.GetByNameAsync("TestDB", cancellationToken);
        return db?.GetSchema(schemaName);
    }

    public Task<Schema?> GetByNameAsync(string databaseName, string schemaName, CancellationToken cancellationToken)
    {
        var db = _databaseRepository.GetByNameAsync(databaseName, cancellationToken).Result;
        return Task.FromResult(db?.GetSchema(schemaName));
    }

    public Task<Schema> UpdateAsync(string databaseName, string schemaName, string newSchemaName, CancellationToken cancellationToken)
    {
        var db = _databaseRepository.GetByNameAsync(databaseName, cancellationToken).Result;
        var schema = db?.GetSchema(schemaName);
        if (schema != null)
        {
            schema.Name = newSchemaName;
            return Task.FromResult(schema);
        }
        throw new Exception($"Schema '{schemaName}' not found in database '{databaseName}'.");
    }

}
