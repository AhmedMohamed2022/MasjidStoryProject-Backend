using Microsoft.EntityFrameworkCore;
using Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Configurations
{
    public class CountryConfiguration: IEntityTypeConfiguration<Country>
    {
        public CountryConfiguration() { }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Fluent API configurations
            builder.Entity<Country>(entity =>
            {
                entity.ToTable("Countries");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.HasIndex(e => e.Name).IsUnique();
            });

            builder.Entity<City>(entity =>
            {
                entity.ToTable("Cities");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.HasIndex(e => new { e.Name, e.CountryId }).IsUnique();
                entity.HasOne(e => e.Country).WithMany(c => c.Cities).HasForeignKey(e => e.CountryId);
            });

            builder.Entity<Masjid>(entity =>
            {
                entity.ToTable("Masjids");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.ShortName).HasMaxLength(100);
                entity.HasOne(e => e.Country).WithMany(c => c.Masjids).HasForeignKey(e => e.CountryId);
                entity.HasOne(e => e.City).WithMany(c => c.Masjids).HasForeignKey(e => e.CityId);
            });

            builder.Entity<Language>(entity =>
            {
                entity.ToTable("Languages");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Code).HasMaxLength(10);
            });

            builder.Entity<MasjidContent>(entity =>
            {
                entity.ToTable("MasjidContents");
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.Masjid).WithMany(m => m.Contents).HasForeignKey(e => e.MasjidId);
                entity.HasOne(e => e.Language).WithMany(l => l.MasjidContents).HasForeignKey(e => e.LanguageId);
            });

            builder.Entity<Media>(entity =>
            {
                entity.ToTable("Media");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.FileUrl).IsRequired();
                entity.HasOne(e => e.Masjid).WithMany(m => m.MediaItems).HasForeignKey(e => e.MasjidId);
            });

            builder.Entity<Story>(entity =>
            {
                entity.ToTable("Stories");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Title).IsRequired().HasMaxLength(255);
                entity.HasOne(e => e.ApplicationUser).WithMany(u => u.Stories).HasForeignKey(e => e.ApplicationUserId);
                entity.HasOne(e => e.Masjid).WithMany(m => m.Stories).HasForeignKey(e => e.MasjidId);
                entity.HasOne(e => e.Language).WithMany(l => l.Stories).HasForeignKey(e => e.LanguageId);
            });

            builder.Entity<Like>(entity =>
            {
                entity.ToTable("Likes");
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.Story).WithMany(s => s.Likes).HasForeignKey(e => e.StoryId);
                entity.HasOne(e => e.User).WithMany(u => u.Likes).HasForeignKey(e => e.UserId);
            });

            builder.Entity<Comment>(entity =>
            {
                entity.ToTable("Comments");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Content).IsRequired();
                entity.HasOne(e => e.Story).WithMany(s => s.Comments).HasForeignKey(e => e.StoryId);
                entity.HasOne(e => e.User).WithMany(u => u.Comments).HasForeignKey(e => e.UserId);
            });

            builder.Entity<Tag>(entity =>
            {
                entity.ToTable("Tags");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.HasIndex(e => e.Name).IsUnique();
            });

            builder.Entity<StoryTag>(entity =>< Paste truncated >
    }
}
