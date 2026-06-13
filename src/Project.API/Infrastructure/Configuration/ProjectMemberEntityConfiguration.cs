using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Project.API.Models.Members;

namespace Project.API.Infrastructure.Configuration
{
    public class ProjectMemberEntityConfiguration : IEntityTypeConfiguration<ProjectMember>
    {
        public void Configure(EntityTypeBuilder<ProjectMember> builder)
        {
            builder.Property(pm => pm.Id)
                .IsRequired();

            builder.Property(pm => pm.UserName)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(pm => pm.Email)
                .IsRequired()
                .HasMaxLength(256);
        }
    }
}
