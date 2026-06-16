using Shared.Services;
using Tasks.API.Infrastructure;

namespace Tasks.API.Apis
{
    public record TaskServices(
        IIdentityService IdentityService,
        ApplicationDbContext Context,
        ILogger<TaskServices> Logger);

}
