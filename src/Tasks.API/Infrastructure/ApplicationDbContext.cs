using Microsoft.EntityFrameworkCore;
using Tasks.API.Infrastructure.Configurations;
using Tasks.API.Models.ProjectMemberhips;
using Tasks.API.Models.ProjectSnapshots;
using Tasks.API.Models.ProjectTasks;

namespace Tasks.API.Infrastructure
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<ProjectTask> Tasks { get; set; } = default!;

        public DbSet<ProjectMembership> ProjectMemberships { get; set; } = default!;

        public DbSet<ProjectSnapshot> ProjectSnapshots { get; set; } = default!;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ProjectTaskEntityConfiguration).Assembly);
        }
    }
}
