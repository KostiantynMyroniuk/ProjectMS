using File.API.Infrastructure;
using File.API.Services;
using Shared.Services;

namespace File.API.Apis
{
    public record FileServices(
        ApplicationDbContext Context,
        ILogger<FileServices> Logger,
        IIdentityService IdentityService,
        IFileStorageService StorageService);

}
