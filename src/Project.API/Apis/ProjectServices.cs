using Project.API.Infrastructure;
using Project.API.Services;

namespace Project.API.Apis
{
    public class ProjectServices(
        IIdentityService identityService, 
        ApplicationDbContext context,
        ILogger<ProjectServices> logger)
    {
        public IIdentityService IdentityService { get; } = identityService;
        public ApplicationDbContext Context { get; } = context;
        public ILogger<ProjectServices> Logger { get; } = logger;
    }
    
}
