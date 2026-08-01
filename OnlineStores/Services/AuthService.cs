using DevOne.Security.Cryptography.BCrypt;

public class AuthService(IJwtTokenService jwtTokenService, IUserRepository userRepository) : IAuthService
{
    private readonly IJwtTokenService _jwtTokenService = jwtTokenService;
    private readonly IUserRepository _userRepository = userRepository;
    public Task<LoginResponse?> LoginAsync(LoginRequest request, CancellationToken cancellationToken)
    {
        var normalizedEmail = request.Email.Trim().ToLowerInvariant();

        var user = _userRepository.GetByEmailAsync(normalizedEmail, cancellationToken).Result;

        if (user is null || !user.IsActive)
        {
            throw new UnauthorizedAccessException("Invalid email or password.");
        }

        var isPasswordValid = BCryptHelper.CheckPassword(request.Password, user.PasswordHash);
        if (!isPasswordValid)
        {
            throw new UnauthorizedAccessException("Invalid email or password.");
        }

        var token = _jwtTokenService.GenerateAccessToken(user);
        var currentUserResponse = new CurrentUserResponse(
            Id: user.Id,
            Email: user.Email,
            FullName: user.FullName,
            Role: user.Role
        );

        var loginResponse = new LoginResponse(
            AccessToken: token.AccessToken,
            ExpiresAt: token.ExpiresAt,
            User: currentUserResponse
        );
        return Task.FromResult<LoginResponse?>(loginResponse);
    }
}
