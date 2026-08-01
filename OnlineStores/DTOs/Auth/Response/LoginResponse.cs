
public sealed record LoginResponse(
    string AccessToken,
    string RefreshToken,
    int AccessTokenExpiresAt,
    int RefreshTokenExpiresAt,
    CurrentUserResponse User);
