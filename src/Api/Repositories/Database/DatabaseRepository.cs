public class DatabaseRepository : IDatabaseRepository
{
    public Task<Database?> GetByNameAsync(string name, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
