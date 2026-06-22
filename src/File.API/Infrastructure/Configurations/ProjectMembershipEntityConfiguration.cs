using File.API.Models.Memberships;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace File.API.Infrastructure.Configurations
{
    public class ProjectMembershipEntityConfiguration : IEntityTypeConfiguration<ProjectMembership>
    {
        public void Configure(EntityTypeBuilder<ProjectMembership> builder)
        {
            builder
                .HasKey(pm => new { pm.UserId, pm.ProjectId });
        }
    }
}
