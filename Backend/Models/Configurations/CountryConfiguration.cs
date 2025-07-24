using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Configurations
{
// EntityTypeConfiguration classes for MasjidStory (Fluent API)
    public class CountryConfiguration: IEntityTypeConfiguration<Country>
    {
            public void Configure(EntityTypeBuilder<Country> builder)
            {
                builder.ToTable("Countries");
                builder.HasKey(c => c.Id);
                // Remove Name property config
                // builder.Property(c => c.Name)
                //        .IsRequired()
                //        .HasMaxLength(100);
                // Relationships
                builder.HasMany(c => c.Contents)
                       .WithOne(cc => cc.Country)
                       .HasForeignKey(cc => cc.CountryId)
                       .OnDelete(DeleteBehavior.Cascade);
                builder.HasMany(c => c.Cities)
                       .WithOne(cy => cy.Country)
                       .HasForeignKey(cy => cy.CountryId)
                       .OnDelete(DeleteBehavior.Cascade);
            }
        }      
}
