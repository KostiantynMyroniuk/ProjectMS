namespace Tasks.API.Models.ProjectSnapshots
{
    public class ProjectSnapshot
    {
        public Guid ProjectId { get; set; }
        public string Name { get; set; } = default!;
        public bool IsActive { get; set; }
    }
}
