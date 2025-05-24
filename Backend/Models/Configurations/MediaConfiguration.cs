using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Configurations
{
    public class MediaConfiguration : IEntityTypeConfiguration<Media>
    {
        public void Configure(EntityTypeBuilder<Media> builder)
        {
            builder.ToTable("Media");
            builder.HasKey(md => md.Id);
            builder.Property(md => md.FileUrl)
                   .IsRequired();
            builder.Property(md => md.MediaType)
                   .HasMaxLength(50)
                   .HasDefaultValue("Image");
            builder.Property(md => md.DateUploaded)
                   .HasDefaultValueSql("GETUTCDATE()");
            builder.HasOne(md => md.Masjid)
                   .WithMany(m => m.MediaItems)
                   .HasForeignKey(md => md.MasjidId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
