using System.ComponentModel;

public sealed record LoginRequest(
    [property: DefaultValue("admin@example.com")] string Email,
    [property: DefaultValue("Password123!")] string Password);
