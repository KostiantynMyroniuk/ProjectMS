using File.API.Models.Projects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace File.API.Infrastructure.Configurations
{
    public class ProjectEntityConfiguration : IEntityTypeConfiguration<ProjectSnapshot>
    {
        public void Configure(EntityTypeBuilder<ProjectSnapshot> builder)
        {
            builder
                .HasKey(ps => ps.ProjectId);
        }
    }
}
