public class UserRepository : IUserRepository
{
    private readonly IReadOnlyCollection<User> _users =
    [
        new User
        {
            Id = Guid.Parse("2f11942f-339e-46dd-b08c-bc540893a807"),
            Email = "admin@onlinestore.com",
            FullName = "Store Administrator",

            PasswordHash = "Admin@123",

            Role = "Admin",
            IsActive = true
        }
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
}
