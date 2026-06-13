using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Project.API.Models.Invitations;
using System.Reflection.Emit;

namespace Project.API.Infrastructure.Configuration
{
    public class ProjectInvitationEntityConfiguration : IEntityTypeConfiguration<ProjectInvitation>
    {
        public void Configure(EntityTypeBuilder<ProjectInvitation> builder)
        {
            builder.Property(i => i.Status)
                .IsRequired()
                .HasConversion<string>();

            builder.Property(i => i.Token)
                .IsRequired();

            builder.Property(i => i.Email)
                .IsRequired()
                .HasMaxLength(256);
        }
    }
}
