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
                builder.Property(c => c.Name)
                       .IsRequired()
                       .HasMaxLength(100);
            // Unique index on Name
            //builder.HasIndex(c => c.Name).IsUnique();
            //    builder.Property(c => c.Code)
            //           .HasMaxLength(10);

                // Relationships
                builder.HasMany(c => c.Cities)
                       .WithOne(cy => cy.Country)
                       .HasForeignKey(cy => cy.CountryId)
                       .OnDelete(DeleteBehavior.Cascade);
            }
        }      
}
