using Project.API.Models.Projects;

namespace Project.API.Models.Members
{
    public class ProjectMember
    {
        public Guid Id { get; set; }

        public DateTime JoinedAt { get; set; }

        public string UserId { get; set; } = default!;

        public string UserName { get; set; } = default!;

        public string Email { get; set; } = default!;


        public Guid ProjectId { get; set; }

        public ProjectModel Project { get; set; } = default!;


        internal static ProjectMember Create(
            string userId,
            string userName,
            string email,
            Guid projectId)
        {
            return new ProjectMember
            {
                JoinedAt = DateTime.UtcNow,
                UserId = userId,
                UserName = userName,
                Email = email,
                ProjectId = projectId
            };
        }
    }
}
