public interface IUserRepository
{
    Task<User?> GetByEmailAsync(
        string email,
        CancellationToken cancellationToken);

    Task<User?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken);
}
