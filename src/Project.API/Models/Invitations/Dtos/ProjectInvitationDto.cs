namespace Project.API.Models.Invitations.Dtos
{
    public class ProjectInvitationDto
    {
        public Guid ProjectId { get; set; }
        public required string Email { get; set; }
    }
}
