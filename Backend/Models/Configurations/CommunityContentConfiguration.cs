using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models.Entities;

namespace Models.Configurations
{
    public class CommunityContentConfiguration : IEntityTypeConfiguration<CommunityContent>
    {
        public void Configure(EntityTypeBuilder<CommunityContent> builder)
        {
            builder.ToTable("CommunityContents");
            builder.HasKey(cc => cc.Id);
            builder.Property(cc => cc.Title)
                .IsRequired()
                .HasMaxLength(150);
            builder.Property(cc => cc.Content)
                .IsRequired()
                .HasMaxLength(500);
            builder.HasOne(cc => cc.Community)
                .WithMany(c => c.Contents)
                .HasForeignKey(cc => cc.CommunityId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(cc => cc.Language)
                .WithMany(l => l.CommunityContents)
                .HasForeignKey(cc => cc.LanguageId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
} 