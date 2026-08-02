public class AuthService(IJwtTokenService jwtTokenService, IUserRepository userRepository) : IAuthService
{
    private readonly IJwtTokenService _jwtTokenService = jwtTokenService;
    private readonly IUserRepository _userRepository = userRepository;

    public async Task<CurrentUserResponse?> GetMeAsync(Guid userId, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(userId, cancellationToken);

        if (user is null || !user.IsActive)
        {
            return null;
        }
        var currentUserResponse = new CurrentUserResponse(
            Id: user.Id,
            Email: user.Email,
            FullName: user.FullName,
            Role: user.Role
        );
        return currentUserResponse;
    }


    public async Task<LoginResponse?> LoginAsync(LoginRequest request, CancellationToken cancellationToken)
    {
        var normalizedEmail = request.Email.Trim().ToLowerInvariant();

        var user = await _userRepository.GetByEmailAsync(normalizedEmail, cancellationToken);

        if (user is null || !user.IsActive)
        {
            return null;
        }

        var isPasswordValid = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);
        if (!isPasswordValid)
        {
            return null;
        }

        var token = _jwtTokenService.GenerateAccessToken(user);
        var refreshToken = Guid.NewGuid().ToString();

        _userRepository.UpdateRefreshTokenAsync(user.Id, refreshToken, DateTime.UtcNow.AddDays(7), cancellationToken).Wait();
        var currentUserResponse = new CurrentUserResponse(
            Id: user.Id,
            Email: user.Email,
            FullName: user.FullName,
            Role: user.Role
        );

        var loginResponse = new LoginResponse(
            AccessToken: token.AccessToken,
            RefreshToken: refreshToken,
            AccessTokenExpiresAt: (int)(token.ExpiresAt - DateTime.UtcNow).TotalMinutes,
            RefreshTokenExpiresAt: (int)(DateTime.UtcNow.AddDays(7) - DateTime.UtcNow).TotalMinutes,
            User: currentUserResponse
        );
        return loginResponse;
    }

    public async Task<RefreshTokenResponse> RefreshTokenAsync(string refreshToken, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByRefreshTokenAsync(refreshToken, cancellationToken);

        if (user is null || !user.IsActive)
        {
            throw new UnauthorizedAccessException("Invalid refresh token.");
        }

        var token = _jwtTokenService.GenerateAccessToken(user);
        var newRefreshToken = Guid.NewGuid().ToString();

        _userRepository.UpdateRefreshTokenAsync(user.Id, newRefreshToken, DateTime.UtcNow.AddDays(7), cancellationToken).Wait();

        var refreshTokenResponse = new RefreshTokenResponse(
            AccessToken: token.AccessToken,
            RefreshToken: newRefreshToken,
            AccessTokenExpiresIn: (int)(token.ExpiresAt - DateTime.UtcNow).TotalMinutes,
            RefreshTokenExpiresIn: (int)(DateTime.UtcNow.AddDays(7) - DateTime.UtcNow).TotalMinutes
        );

        return refreshTokenResponse;
    }


    public async Task<RegisterResponse> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken)
    {
        var normalizedEmail = request.Email.Trim().ToLowerInvariant();

        var existingUser = await _userRepository.GetByEmailAsync(normalizedEmail, cancellationToken);
        if (existingUser != null)
        {
            throw new InvalidOperationException("User already exists.");
        }

        var user = User.Create(
            email: normalizedEmail,
            fullName: request.FullName,
            passwordHash: BCrypt.Net.BCrypt.HashPassword(request.Password, BCrypt.Net.BCrypt.GenerateSalt()),
            role: "User",
            isActive: true
        );

        await _userRepository.AddAsync(user, cancellationToken);

        var currentUserResponse = new CurrentUserResponse(
            Id: user.Id,
            Email: user.Email,
            FullName: user.FullName,
            Role: user.Role
        );

        return new RegisterResponse(currentUserResponse);
    }

}
