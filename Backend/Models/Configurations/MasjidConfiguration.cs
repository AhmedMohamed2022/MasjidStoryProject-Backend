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
    public class MasjidConfiguration : IEntityTypeConfiguration<Masjid>
    {
        public void Configure(EntityTypeBuilder<Masjid> builder)
        {
            builder.ToTable("Masjids");
            builder.HasKey(m => m.Id);
            builder.Property(m => m.ArchStyle)
                   .HasMaxLength(100);
            builder.Property(m => m.DateOfRecord)
                   .HasDefaultValueSql("GETUTCDATE()");
            // Location
            builder.Property(m => m.Latitude)
                   .HasColumnType("decimal(9,6)");
            builder.Property(m => m.Longitude)
                   .HasColumnType("decimal(9,6)");
            // Relationships
            builder.HasOne(m => m.Country)
                   .WithMany(c => c.Masjids)
                   .HasForeignKey(m => m.CountryId)
                   .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(m => m.City)
                   .WithMany(c => c.Masjids)
                   .HasForeignKey(m => m.CityId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }

}
