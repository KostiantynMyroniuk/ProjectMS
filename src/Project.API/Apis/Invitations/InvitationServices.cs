using Microsoft.Extensions.Options;
using Project.API.Infrastructure;
using Project.API.Models;
using Shared.Services;

namespace Project.API.Apis.Invitations
{
    public record InvitationServices(
        ApplicationDbContext Context,
        IIdentityService IdentityService,
        ILogger<InvitationServices> Logger,
        IOptions<AppOptions> AppOptions);
}
