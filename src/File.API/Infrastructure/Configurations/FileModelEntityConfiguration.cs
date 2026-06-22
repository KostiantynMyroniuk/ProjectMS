using File.API.Models.Files;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace File.API.Infrastructure.Configurations
{
    public class FileModelEntityConfiguration : IEntityTypeConfiguration<FileModel>
    {
        public void Configure(EntityTypeBuilder<FileModel> builder)
        {
            builder.Property(ps => ps.OriginalName)
                .IsRequired()
                .HasMaxLength(50);
        }
    }
}
