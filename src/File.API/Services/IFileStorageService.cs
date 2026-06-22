namespace File.API.Services
{
    public interface IFileStorageService
    {
        Task<string> UploadFileAsync(IFormFile file, CancellationToken cancellationToken = default);
        Uri GetFileUri(string fileName, CancellationToken cancellationToken = default);
        Task DeleteFileAsync(string fileName, CancellationToken cancellationToken = default);
    }
}
