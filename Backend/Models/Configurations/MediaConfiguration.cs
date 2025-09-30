using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models.Entities;

namespace Models.Configurations
{
    public class MediaConfiguration : IEntityTypeConfiguration<Media>
    {
        public void Configure(EntityTypeBuilder<Media> builder)
        {
            builder.ToTable("Media");
            builder.HasKey(m => m.Id);

            builder.Property(m => m.FileUrl)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(m => m.FileName)
                .HasMaxLength(255);

            builder.Property(m => m.ContentType)
                .HasMaxLength(100);

            builder.Property(m => m.FileSize)
                .IsRequired();

            builder.Property(m => m.MediaType)
                .HasMaxLength(50)
                .HasDefaultValue("Image");

            builder.Property(m => m.DateUploaded)
                .HasDefaultValueSql("GETUTCDATE()");
            // Foreign key relationships with cascade delete
            builder.HasOne<Masjid>()
                .WithMany(m => m.MediaItems)
                .HasForeignKey(m => m.MasjidId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(false);

            builder.HasOne<Story>()
                .WithMany(s => s.MediaItems)
                .HasForeignKey(m => m.StoryId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(false);

            // Indexes for better performance
            builder.HasIndex(m => m.MasjidId);
            builder.HasIndex(m => m.StoryId);
            builder.HasIndex(m => m.DateUploaded);
        }
    }
}