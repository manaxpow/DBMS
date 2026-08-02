using Microsoft.AspNetCore.Http;

public class FileStorageService : IFileStorageService
{
    public Task<string> UploadFileAsync(IFormFile file, string folder, CancellationToken cancellationToken = default)
    {
        // Mock implementation
        var fileName = $"{Guid.NewGuid()}_{file.FileName}";
        return Task.FromResult($"/images/{folder}/{fileName}");
    }

    public Task DeleteFileAsync(string fileUrl, CancellationToken cancellationToken = default)
    {
        // Mock implementation
        return Task.CompletedTask;
    }
}
