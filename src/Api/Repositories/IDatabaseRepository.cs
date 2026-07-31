using System.Text.Json;

public interface IDatabaseRepository
{
    Task<Database?> GetByNameAsync(string name, CancellationToken cancellationToken);
}

public class DatabaseRepository : IDatabaseRepository
{
    private readonly string _filePath;
    private readonly JsonSerializerOptions _serializerOptions;

    public DatabaseRepository(string filePath, JsonSerializerOptions serializerOptions)
    {
        _filePath = filePath;
        _serializerOptions = serializerOptions;
    }

    public Task<Database?> GetByNameAsync(string name, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
