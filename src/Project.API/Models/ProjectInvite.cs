namespace Project.API.Models
{
    public class ProjectInvite
    {
        public Guid Id { get; set; }

        public string Token { get; set; }

        public InviteStatus Status { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime ExpiresAt { get; set; }

        public DateTime? AcceptedAt { get; set; }
        

        public string InvitedByUserId { get; set; }

        public Guid ProjectId { get; set; }

        public ProjectModel Project { get; set; }
    }

    public enum InviteStatus
    {
        Pending = 1,
        Accepted = 2,
        Declined = 3,
        Expired = 4
    }
}
