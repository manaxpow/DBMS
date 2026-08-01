public record RefreshTokenResponse(
    string AccessToken,
    string RefreshToken,
    int AccessTokenExpiresIn,
    int RefreshTokenExpiresIn
);
