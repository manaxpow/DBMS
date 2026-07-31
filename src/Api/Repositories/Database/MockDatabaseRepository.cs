public sealed class MockDatabaseRepository : IDatabaseRepository
{
    private readonly List<Database> _databases;

    public MockDatabaseRepository()
    {
        var database = new RelationalDatabase { Name = "test" };

        var schema = new Schema("test");
        var table = new Table("test");

        schema.AddTable(table);
        database.AddSchema(schema);

        _databases = new List<Database> { database };
    }

    public Task<Database?> GetByNameAsync(string name, CancellationToken cancellationToken)
    {
        var db = _databases.FirstOrDefault(d => d.Name == name);
        return Task.FromResult(db);
    }
}
