using File.API.Infrastructure.Configurations;
using File.API.Models.Files;
using File.API.Models.Memberships;
using File.API.Models.Projects;
using Microsoft.EntityFrameworkCore;

namespace File.API.Infrastructure
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<FileModel> Files { get; set; } = default!;

        public DbSet<ProjectSnapshot> ProjectSnapshots { get; set; } = default!;

        public DbSet<ProjectMembership> ProjectMemberships { get; set; } = default!;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(FileModelEntityConfiguration).Assembly);
        }
    }
}
