using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models.Entities;

namespace Models.Configurations
{
    public class StoryContentConfiguration : IEntityTypeConfiguration<StoryContent>
    {
        public void Configure(EntityTypeBuilder<StoryContent> builder)
        {
            builder.ToTable("StoryContents");

            builder.HasKey(sc => sc.Id);

            builder.Property(sc => sc.Title)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(sc => sc.Content)
                .IsRequired();

            builder.HasOne(sc => sc.Story)
                .WithMany(s => s.Contents)
                .HasForeignKey(sc => sc.StoryId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(sc => sc.Language)
                .WithMany()
                .HasForeignKey(sc => sc.LanguageId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
} 