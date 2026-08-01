public sealed record JwtTokenResult(
    string AccessToken,
    DateTime ExpiresAt);
