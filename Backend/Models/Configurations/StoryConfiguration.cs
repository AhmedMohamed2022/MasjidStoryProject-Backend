﻿using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Configurations
{
    public class StoryConfiguration : IEntityTypeConfiguration<Story>
    {
        public void Configure(EntityTypeBuilder<Story> builder)
        {
            builder.ToTable("Stories");

            builder.HasKey(s => s.Id);

            builder.Property(s => s.DatePublished)
                .HasDefaultValueSql("GETDATE()");

            builder.Property(s => s.IsApproved)
                .HasDefaultValue(false);

            builder.HasOne(s => s.ApplicationUser)
                .WithMany(u => u.Stories)
                .HasForeignKey(s => s.ApplicationUserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(s => s.Masjid)
                .WithMany(m => m.Stories)
                .HasForeignKey(s => s.MasjidId)
                .OnDelete(DeleteBehavior.Restrict);

            // Add Contents relationship
            builder.HasMany(s => s.Contents)
                .WithOne(sc => sc.Story)
                .HasForeignKey(sc => sc.StoryId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }

}
