using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.Entities;

namespace Models.Configurations
{
    public class CityConfiguration:IEntityTypeConfiguration<City>
    {
        public void Configure(EntityTypeBuilder<City> builder)
        {
            builder.ToTable("Cities");
            builder.HasKey(c => c.Id);
            // Remove Name property config
            // builder.Property(c => c.Name)
            //        .IsRequired()
            //        .HasMaxLength(100);
            // Relationships
            builder.HasMany(c => c.Contents)
                   .WithOne(cc => cc.City)
                   .HasForeignKey(cc => cc.CityId)
                   .OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(c => c.Country)
                   .WithMany(ct => ct.Cities)
                   .HasForeignKey(c => c.CountryId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
