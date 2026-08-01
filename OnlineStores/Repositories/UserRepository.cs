public class UserRepository : IUserRepository
{
    private readonly List<User> _users =
    [
        User.Create
        (
            email: "admin@onlinestore.com",
            fullName: "Store Administrator",
            passwordHash: "Admin@123",
            role: "Admin",
            isActive: true
        )
    ];

    public Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken)
    {
        var user = _users.FirstOrDefault(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
        return Task.FromResult(user);
    }

    public Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var user = _users.FirstOrDefault(u => u.Id == id);
        return Task.FromResult(user);
    }

    public Task UpdateRefreshTokenAsync(Guid userId, string refreshToken, DateTime refreshTokenExpiryTime, CancellationToken cancellationToken)
    {
        var user = _users.FirstOrDefault(u => u.Id == userId);
        if (user != null)
        {
            user.SetRefreshToken(refreshToken, refreshTokenExpiryTime);
        }
        return Task.CompletedTask;
    }

    public Task AddAsync(User user, CancellationToken cancellationToken)
    {
        _users.Add(user);
        return Task.CompletedTask;
    }

    public Task<User?> GetByRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken)
    {
        var user = _users.FirstOrDefault(u => u.RefreshToken == refreshToken && u.RefreshTokenExpiryTime > DateTime.UtcNow);
        return Task.FromResult(user);
    }

}
