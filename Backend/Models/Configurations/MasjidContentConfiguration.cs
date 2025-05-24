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
    public class MasjidContentConfiguration : IEntityTypeConfiguration<MasjidContent>
    {
        public void Configure(EntityTypeBuilder<MasjidContent> builder)
        {
            builder.ToTable("MasjidContents");
            builder.HasKey(mc => mc.Id);
            builder.Property(mc => mc.Name)
                   .HasMaxLength(255);
            builder.Property(mc => mc.Description)
                   .IsRequired();
            builder.HasOne(mc => mc.Masjid)
                   .WithMany(m => m.Contents)
                   .HasForeignKey(mc => mc.MasjidId)
                   .OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(mc => mc.Language)
                   .WithMany(l => l.MasjidContents)
                   .HasForeignKey(mc => mc.LanguageId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
