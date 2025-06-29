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
    public class LikeConfiguration : IEntityTypeConfiguration<Like>
    {
        public void Configure(EntityTypeBuilder<Like> builder)
        {
            builder.ToTable("Likes");

            builder.HasKey(l => l.Id);

            builder.Property(l => l.ContentId)
                .IsRequired();

            builder.Property(l => l.ContentType)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(l => l.DateLiked)
                .HasDefaultValueSql("GETDATE()");

            builder.HasOne(l => l.User)
                .WithMany()
                .HasForeignKey(l => l.UserId);

            // Optional relationship for backward compatibility
            builder.HasOne(l => l.Story)
                .WithMany(s => s.Likes)
                .HasForeignKey(l => l.StoryId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired(false);
        }
    }
}
