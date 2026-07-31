using System.Threading;
using System.Threading.Tasks;

public sealed class MockSchemaRepository : ISchemaRepository
{
    private readonly IDatabaseRepository _databaseRepository;

    public MockSchemaRepository(IDatabaseRepository databaseRepository)
    {
        _databaseRepository = databaseRepository;
    }

    public async Task<Schema?> GetByNameAsync(int databaseId, string schemaName, CancellationToken cancellationToken)
    {
        var db = await _databaseRepository.GetByNameAsync("TestDB", cancellationToken);
        return db?.GetSchema(schemaName);
    }
}
