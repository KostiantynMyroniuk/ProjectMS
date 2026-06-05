namespace Project.API.Models
{
    public class ProjectMember
    {
        public Guid Id { get; set; }

        public DateTime JoinedAt { get; set; }

        public string UserId { get; set; } = default!;

        public string UserName { get; set; } = default!;


        public Guid ProjectId { get; set; }

        public ProjectModel Project { get; set; } = default!;


        internal static ProjectMember Create(
            Guid id,
            string userId,
            string userName)
        {
            return new ProjectMember
            {
                Id = id,
                JoinedAt = DateTime.UtcNow,
                UserId = userId,
                UserName = userName
            };
        }
    }
}
