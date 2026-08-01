public sealed class User
{
    public Guid Id { get; init; }

    public string Email { get; private set; } = string.Empty;

    public string FullName { get; private set; } = string.Empty;

    public string PasswordHash { get; private set; } = string.Empty;
    public string RefreshToken { get; private set; } = string.Empty;
    public DateTime RefreshTokenExpiryTime { get; private set; }
    public string Role { get; private set; } = "User";

    public bool IsActive { get; private set; } = true;
    private User()
    {
    }

    public static User Create(string email, string fullName, string passwordHash, string role = "User", bool isActive = true)
    {
        return new User
        {
            Id = Guid.NewGuid(),
            Email = email,
            FullName = fullName,
            PasswordHash = passwordHash,
            Role = role,
            IsActive = true
        };
    }

    public void SetRefreshToken(string refreshToken, DateTime expiryTime)
    {
        RefreshToken = refreshToken;
        RefreshTokenExpiryTime = expiryTime;
    }
}
