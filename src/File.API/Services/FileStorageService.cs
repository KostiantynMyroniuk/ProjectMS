using Azure.Storage.Blobs;
using File.API.Models;
using Microsoft.Extensions.Options;
using MassTransit.Configuration;
using Azure.Storage.Sas;

namespace File.API.Services
{
    public class FileStorageService : IFileStorageService
    {
        private readonly BlobServiceClient _blobServiceClient;
        private readonly IOptions<BlobOptions> _blobOptions;
        
        public FileStorageService(
            BlobServiceClient blobServiceClient,
            IOptions<BlobOptions> blobOptions)
        {
            _blobServiceClient = blobServiceClient;
            _blobOptions = blobOptions;
        }

        public Uri GetFileUri(string fileName, CancellationToken cancellationToken = default)
        {
            var container = _blobServiceClient.GetBlobContainerClient(_blobOptions.Value.ContainerName);

            var blob = container.GetBlobClient(fileName);
            var url = blob.GenerateSasUri(
                BlobSasPermissions.Read,
                DateTime.UtcNow.AddMinutes(15));

            return url;
        }

        public async Task<string> UploadFileAsync(IFormFile file, CancellationToken cancellationToken = default)
        {
            var container = _blobServiceClient.GetBlobContainerClient(_blobOptions.Value.ContainerName);

            var fileName = $"{Guid.NewGuid().ToString()}_{file.FileName}";
            var blob = container.GetBlobClient(fileName);

            await using var stream = file.OpenReadStream();
            await blob.UploadAsync(stream, cancellationToken);

            return fileName;
        }

        public async Task DeleteFileAsync(string fileName, CancellationToken cancellationToken = default)
        {
            var container = _blobServiceClient.GetBlobContainerClient(_blobOptions.Value.ContainerName);
            var blob = container.GetBlobClient(fileName);

            await blob.DeleteIfExistsAsync(cancellationToken: cancellationToken);
        }
    }
}
