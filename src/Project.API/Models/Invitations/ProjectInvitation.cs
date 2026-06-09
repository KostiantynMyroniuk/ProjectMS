using Project.API.Models.Projects;

namespace Project.API.Models.Invitations
{
    public class ProjectInvitation
    {
        public Guid Id { get; set; }

        public string Token { get; set; }

        public string Email { get; set; }

        public InviteStatus Status { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime ExpiresAt { get; set; }

        public DateTime? AcceptedAt { get; set; }
        
        public string InvitedByUserId { get; set; }


        public Guid ProjectId { get; set; }

        public ProjectModel Project { get; set; } = null!;


        public ProjectInvitation(
            string token,
            string email,
            string invitedByUserId,
            Guid projectId)
        {
            Id = Guid.CreateVersion7();
            Token = token;
            Email = email;
            Status = InviteStatus.Pending;
            InvitedByUserId = invitedByUserId;
            ProjectId = projectId;
            CreatedAt = DateTime.UtcNow;
            ExpiresAt = DateTime.UtcNow.AddDays(7);
        }
    }

    public enum InviteStatus
    {
        Pending = 1,
        Accepted = 2,
        Declined = 3,
        Expired = 4
    }
}
