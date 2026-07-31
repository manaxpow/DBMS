public sealed class MockDatabaseRepository : IDatabaseRepository
{
    private readonly List<Database> _databases;

    public MockDatabaseRepository()
    {
        var database = new RelationalDatabase { Name = "TestDB" };

        var schema = new Schema("public");
        var table = new Table("TestTable");

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
