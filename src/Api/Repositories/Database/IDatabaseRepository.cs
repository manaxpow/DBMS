using System.Text.Json;

public interface IDatabaseRepository
{
    Task<Database?> GetByNameAsync(string name, CancellationToken cancellationToken);
}
