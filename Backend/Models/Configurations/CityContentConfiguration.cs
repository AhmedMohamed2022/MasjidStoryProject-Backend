using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models.Entities;

namespace Models.Configurations
{
    public class CityContentConfiguration : IEntityTypeConfiguration<CityContent>
    {
        public void Configure(EntityTypeBuilder<CityContent> builder)
        {
            builder.ToTable("CityContents");
            builder.HasKey(cc => cc.Id);
            builder.Property(cc => cc.Name)
                   .IsRequired()
                   .HasMaxLength(100);
            builder.HasOne(cc => cc.City)
                   .WithMany(c => c.Contents)
                   .HasForeignKey(cc => cc.CityId)
                   .OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(cc => cc.Language)
                   .WithMany()
                   .HasForeignKey(cc => cc.LanguageId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
} 