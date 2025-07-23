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
    public class TagContentConfiguration : IEntityTypeConfiguration<TagContent>
    {
        public void Configure(EntityTypeBuilder<TagContent> builder)
        {
            builder.ToTable("TagContents");

            builder.HasKey(tc => tc.Id);

            builder.Property(tc => tc.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.HasOne(tc => tc.Tag)
                .WithMany(t => t.Contents)
                .HasForeignKey(tc => tc.TagId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(tc => tc.Language)
                .WithMany()
                .HasForeignKey(tc => tc.LanguageId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
} 