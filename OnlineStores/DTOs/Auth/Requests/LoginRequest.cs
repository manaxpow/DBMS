using System.ComponentModel;

public sealed record LoginRequest(
    [property: DefaultValue("example@gmail.com")] string Email,
    [property: DefaultValue("password")] string Password);
