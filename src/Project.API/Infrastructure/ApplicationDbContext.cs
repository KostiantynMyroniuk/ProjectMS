using Microsoft.EntityFrameworkCore;
using Project.API.Models;

namespace Project.API.Infrastructure
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<ProjectModel> Projects { get; set; }

        public DbSet<ProjectMember> Members { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProjectModel>()
                .HasMany(pm => pm.ProjectMembers)
                .WithOne(m => m.Project)
                .HasForeignKey(pm => pm.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
