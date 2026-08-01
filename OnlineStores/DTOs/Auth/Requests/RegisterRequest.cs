using System.ComponentModel;

public record RegisterRequest(
    [property: DefaultValue("example@gmail.com")] string Email,
    [property: DefaultValue("password")] string Password,
    [property: DefaultValue("John Doe")] string FullName
);
