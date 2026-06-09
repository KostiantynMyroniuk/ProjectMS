using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Project.API.Models.Invitations;
using Project.API.Models.Invitations.Dtos;
using Project.API.Models.Projects;

namespace Project.API.Apis.Invitations
{
    public static class InvitationApi
    {
        public static IEndpointRouteBuilder MapInvitationApi(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/").RequireAuthorization();

            group.MapPost("/invite", InviteUser);
            group.MapGet("/invitation/accept", AcceptInvitation);

            return app;
        }

        public static async Task<IResult> InviteUser(
            ProjectInvitationDto invitationDto,
            [AsParameters] InvitationServices services)
        {
            var userId = services.IdentityService.GetUserId();
            var token = Guid.NewGuid();

            var project = await services.Context.Projects
                .FindAsync(invitationDto.ProjectId);

            if (project == null)
            {
                return Results.NotFound("Project not found");
            }

            var existing = await services.Context.Invitations
                .AnyAsync(i => i.Email == invitationDto.Email 
                          && i.ProjectId == invitationDto.ProjectId 
                          && i.Status == InviteStatus.Pending);

            if (existing)
            {
                return Results.Conflict("Invitation already sent");
            }

            var invitation = new ProjectInvitation(
                token: token.ToString(),
                email: invitationDto.Email,
                invitedByUserId: userId,
                projectId: invitationDto.ProjectId);

            services.Context.Invitations.Add(invitation);
            await services.Context.SaveChangesAsync();

            var baseUrl = services.AppOptions.Value.BaseUrl;

            var acceptUrl = new Uri($"{baseUrl}/invitation/accept?token={token}");

            return Results.Ok(new { acceptUrl });
        }

        public static async Task<IResult> AcceptInvitation(
            string token,
            [AsParameters] InvitationServices services)
        {
            var userId = services.IdentityService.GetUserId();
            var userEmail = services.IdentityService.GetEmail();
            var userName = services.IdentityService.GetUserName();

            var invitation = await services.Context.Invitations
                .Include(i => i.Project)
                    .ThenInclude(p => p.ProjectMembers)
                .FirstOrDefaultAsync(i => i.Token == token);
            
            if (invitation == null)
            {
                return Results.NotFound();
            }

            if (invitation.Email != userEmail)
            {
                return Results.NotFound();
            }

            if (invitation.Status != InviteStatus.Pending)
            {
                return Results.BadRequest("Invite already used");
            }

            if (invitation.ExpiresAt < DateTime.UtcNow)
            {
                invitation.Status = InviteStatus.Expired;

                return Results.BadRequest("Invite is expired");
            }

            var result = invitation.Project.AddMember(
                userId: userId, 
                userName: userName, 
                email: userEmail);

            if (!result.IsSuccess)
            {
                return Results.BadRequest(result.ErrorMessage);
            }

            invitation.Status = InviteStatus.Accepted;
            invitation.AcceptedAt = DateTime.UtcNow;

            await services.Context.SaveChangesAsync();

            services.Logger.LogInformation("User {UserId} accepted invitation to project {ProjectId}", userId, invitation.ProjectId);

            return Results.Ok();
        }
    }
}
