namespace File.API.Models.Memberships
{
    public class ProjectMembership
    {
        public string UserId { get; set; } = default!;
        public Guid ProjectId { get; set; }
    }
}
