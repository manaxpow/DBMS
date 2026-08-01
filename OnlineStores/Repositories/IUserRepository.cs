public interface IUserRepository
{
    Task<User?> GetByEmailAsync(
        string email,
        CancellationToken cancellationToken);

    Task<User?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken);

    Task UpdateRefreshTokenAsync(
        Guid userId,
        string refreshToken,
        DateTime refreshTokenExpiryTime,
        CancellationToken cancellationToken);

    Task AddAsync(
        User user,
        CancellationToken cancellationToken);

    Task<User?> GetByRefreshTokenAsync(
        string refreshToken,
        CancellationToken cancellationToken);
}
