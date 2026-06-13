using Microsoft.EntityFrameworkCore;
using Project.API.Infrastructure.Configuration;
using Project.API.Models.Invitations;
using Project.API.Models.Members;
using Project.API.Models.Projects;

namespace Project.API.Infrastructure
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<ProjectModel> Projects { get; set; } = default!;

        public DbSet<ProjectMember> ProjectMembers { get; set; } = default!;

        public DbSet<ProjectInvitation> Invitations { get; set; } = default!;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ProjectEntityConfiguration).Assembly);
        }
    }
}
