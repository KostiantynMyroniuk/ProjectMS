using Microsoft.EntityFrameworkCore;
using Project.API.Models.Invitations;
using Project.API.Models.Members;
using Project.API.Models.Projects;

namespace Project.API.Infrastructure
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<ProjectModel> Projects { get; set; }

        public DbSet<ProjectMember> ProjectMembers { get; set; }

        public DbSet<ProjectInvitation> Invitations { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProjectModel>()
                .HasMany(pm => pm.ProjectMembers)
                .WithOne(m => m.Project)
                .HasForeignKey(pm => pm.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ProjectInvitation>()
                .Property(i => i.Status)
                .HasConversion<string>();
        }
    }
}
