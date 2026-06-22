namespace File.API.Models.Projects
{
    public class ProjectSnapshot
    {
        public Guid ProjectId { get; set; }
        public string Name { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
}
