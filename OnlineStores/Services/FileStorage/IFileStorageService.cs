using Microsoft.AspNetCore.Http;

public interface IFileStorageService
{
    Task<string> UploadFileAsync(IFormFile file, string folder, CancellationToken cancellationToken = default);
    Task DeleteFileAsync(string fileUrl, CancellationToken cancellationToken = default);
}
