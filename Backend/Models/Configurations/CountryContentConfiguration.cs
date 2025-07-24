using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models.Entities;

namespace Models.Configurations
{
    public class CountryContentConfiguration : IEntityTypeConfiguration<CountryContent>
    {
        public void Configure(EntityTypeBuilder<CountryContent> builder)
        {
            builder.ToTable("CountryContents");
            builder.HasKey(cc => cc.Id);
            builder.Property(cc => cc.Name)
                   .IsRequired()
                   .HasMaxLength(100);
            builder.HasOne(cc => cc.Country)
                   .WithMany(c => c.Contents)
                   .HasForeignKey(cc => cc.CountryId)
                   .OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(cc => cc.Language)
                   .WithMany()
                   .HasForeignKey(cc => cc.LanguageId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
} 