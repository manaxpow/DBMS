public sealed record CurrentUserResponse(
    Guid Id,
    string Email,
    string FullName,
    string Role);
