public interface IAuthService
{
    Task<LoginResponse?> LoginAsync(
        LoginRequest request,
        CancellationToken cancellationToken);

    Task<RegisterResponse> RegisterAsync(
        RegisterRequest request,
        CancellationToken cancellationToken);

    Task<RefreshTokenResponse> RefreshTokenAsync(
        string refreshToken,
        CancellationToken cancellationToken);

    Task<CurrentUserResponse?> GetMeAsync(
        Guid userId,
        CancellationToken cancellationToken);
}
