namespace Tasks.API.Models.ProjectMemberhips
{
    public class ProjectMembership
    {
        public string UserId { get; set; } = default!;
        public Guid ProjectId { get; set; }
    }
}
