using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tasks.API.Models.ProjectSnapshots;

namespace Tasks.API.Infrastructure.Configurations
{
    public class ProjectSnapshotEntityConfiguration : IEntityTypeConfiguration<ProjectSnapshot>
    {
        public void Configure(EntityTypeBuilder<ProjectSnapshot> builder)
        {
            builder.HasKey(ps => ps.ProjectId);
        }
    }
}
