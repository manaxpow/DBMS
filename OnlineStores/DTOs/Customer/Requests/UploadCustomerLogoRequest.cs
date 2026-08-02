using Microsoft.AspNetCore.Http;

public sealed class UploadCustomerLogoRequest
{
    public required IFormFile File { get; init; }
}
