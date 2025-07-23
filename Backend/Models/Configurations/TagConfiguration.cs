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
    public class TagConfiguration : IEntityTypeConfiguration<Tag>
    {
        public void Configure(EntityTypeBuilder<Tag> builder)
        {
            builder.ToTable("Tags");

            builder.HasKey(t => t.Id);

            // Remove flat Name property config
            // builder.Property(t => t.Name)
            //     .IsRequired()
            //     .HasMaxLength(100);

            builder.HasMany(t => t.Contents)
                .WithOne(tc => tc.Tag)
                .HasForeignKey(tc => tc.TagId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }

}
