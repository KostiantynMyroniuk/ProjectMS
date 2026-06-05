using Shared.Results;
using System.ComponentModel.DataAnnotations;

namespace Project.API.Models
{
    public class ProjectModel
    {
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; } = default!;

        public string? Description { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }


        public string OwnerId { get; set; } = default!;

        public string OwnerName { get; set; } = default!;


        public IList<ProjectMember> ProjectMembers { get; private set; } = new List<ProjectMember>();


        public ProjectModel(string name)
        {
            Id = Guid.CreateVersion7();
            Name = name;
            CreatedAt = DateTime.UtcNow;
        }

        public void Update(
            string name,
            string? description)
        {
            Name = name;
            Description = description;
            UpdatedAt = DateTime.UtcNow;
        }

        public Result AddMember(string userId, string userName)
        {
            if (ProjectMembers.Any(m => m.UserId == userId))
                return Result.Failure("User is already a member of the project.");

            var member = ProjectMember.Create(Id, userId, userName);

            ProjectMembers.Add(member);

            return Result.Success();
        }
    }
}
