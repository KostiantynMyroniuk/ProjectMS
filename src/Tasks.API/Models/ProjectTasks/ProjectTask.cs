using Tasks.API.Models.Enums;

namespace Tasks.API.Models.ProjectTasks
{
    public class ProjectTask
    {
        public Guid Id { get; set; }

        public Guid ProjectId { get; set; }

        public string? AssignedToUserId { get; set; } = default!;

        public string Title { get; set; } = default!;

        public string? Description { get; set; } = default!;

        public Status Status { get; set; }

        public Priority Priority { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? DueDate { get; set; }


        public Guid? ParentTaskId { get; set; }
        public ProjectTask? ParentTask { get; set; }
        public ICollection<ProjectTask> SubTasks { get; set; } = new List<ProjectTask>();


        public ProjectTask (string title)
        {
            Id = Guid.CreateVersion7();
            Title = title;
            Status = Status.ToDo;
            CreatedAt = DateTime.UtcNow;
        }
    }
}
