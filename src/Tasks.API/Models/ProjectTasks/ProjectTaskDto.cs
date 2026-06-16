using Tasks.API.Models.Enums;

namespace Tasks.API.Models.ProjectTasks
{
    public class ProjectTaskDto
    {
        public Guid Id { get; set; }

        public string Title { get; set; } = default!;

        public string? Description { get; set; } = default!;

        public Status Status { get; set; }

        public Priority Priority { get; set; }
    }
}
