using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tasks.API.Models.ProjectMemberhips;

namespace Tasks.API.Infrastructure.Configurations
{
    public class ProjectMembersipEntityConfiguration : IEntityTypeConfiguration<ProjectMembership>
    {
        public void Configure(EntityTypeBuilder<ProjectMembership> builder)
        {
            builder.HasKey(pm => new { pm.UserId, pm.ProjectId });
        }
    }
}
